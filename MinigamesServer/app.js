var express = require('express')
    , bodyParser = require('body-parser')
    , expressValidator = require('express-validator')
    , redis = require('redis')
    , Sequelize = require('sequelize')
    , moment = require('moment')
    , http = require('http')
    , fs = require('fs')
    , app = express()
    , server = http.createServer(app)
    , io = require('socket.io').listen(server)
    , queue = [];

var redisEndpoint = {
    host: 'minigamesredis.ljan5k.0001.apn2.cache.amazonaws.com',
    port: 6379
};
var rdsEndpoint = {
    host: 'minigames.cgywyyk9oy0p.ap-northeast-2.rds.amazonaws.com',
    port: 3306
};

var redisClient = redis.createClient(redisEndpoint.port, redisEndpoint.host);

// MySQL DB 이름, 계정, 암호
var sequelize = new Sequelize('Minigames', 'admin', 'wlsdn123!', {
    host: rdsEndpoint.host,
    port: rdsEndpoint.port
});

// USER_INFO 테이블 정의
var User = sequelize.define('UserInfo', {
    user_id: { type: Sequelize.STRING, allowNull: false, primaryKey: true },
    password: { type: Sequelize.STRING, allowNull: false },
    name: { type: Sequelize.STRING, allowNull: false },
    email: Sequelize.STRING,
    total_score: { type: Sequelize.INTEGER, allowNull: false },
    star_point: { type: Sequelize.INTEGER, allowNull: false }
}, {
    timestamps: false,
    tableName: 'USER_INFO'
});

// USER_SCORE 테이블 정의
var UserScore = sequelize.define('UserScore', {
    user_id: { type: Sequelize.STRING, allowNull: false, primaryKey: true },
    game_name: { type: Sequelize.STRING, allowNull: false, primaryKey: true },
    score: { type: Sequelize.INTEGER, allowNull: false }
}, {
    timestamps: false,
    tableName: 'USER_SCORE'
});

// USER_ITEM 테이블 정의
var UserItem = sequelize.define('UserItem', {
    user_id: { type: Sequelize.STRING, allowNull: false, primaryKey: true },
    item_id: { type: Sequelize.INTEGER(3).ZEROFILL, allowNull: false, primaryKey: true },
    point: { type: Sequelize.INTEGER, allowNull: false }
}, {
    timestamps: false,
    tableName: 'USER_ITEM'
});

app.use(bodyParser.urlencoded());
app.use(bodyParser.json());
app.use(expressValidator());

// test
app.get('/test', function (req, res) {
	res.send('Hello World');
});

// 유저 정보 얻기 [USER_INFO]
app.get('/users/:userId', function (req, res) {
    req.assert('userId').notEmpty();
    var errors = req.validationErrors();
    if (errors) {
        res.send({ error: 1, data: errors });
        return;
    }

    var userId = req.params.userId;

    User.find({ where: { user_id: userId } }).then(function (userInfo) {
        if (userInfo)
            res.send({ error: 0, data: JSON.stringify(userInfo) });
        else
            res.send({ error: 0, data: '' });
    }).catch(function (error) {
        res.send({ error: 2, data: errors });
    });
});

// 클라이언트에서 점수 받기 [USER_SCORE]
app.post('/users/:userId/send/score', function (req, res) {
    req.assert('userId').notEmpty();
    req.checkBody('gameName').notEmpty();
    req.checkBody('score').notEmpty();
    var errors = req.validationErrors();
    if (errors) {
        res.send({ error: 1, data: errors });
        return;
    }

    var userId = req.params.userId;
    var gameName = req.body.gameName;
    var score = req.body.score;

    redisClient.zscore(gameName + '_rank', userId, function (err, reply) {
        if (!err) {
            if (score > reply) {
                var inc = score - reply
                redisClient.zadd(gameName + '_rank', score, userId, function (err, reply) {
                    if (!err) {
                        UserScore.upsert(
                            { user_id: userId, game_name: gameName,  score: score },
                            { where: { user_id: userId, game_name: gameName } }
                        ).then(function (test) {
                            //res.send({ error: 0 });
                        }).catch(function (error) {
                            res.send({ error: 2, data: error });
                        });
                    } else {
                        res.send({ error: 3, data: err });
                        return;
                    }
                });
                redisClient.zincrby('total_rank', inc, userId, function (err, reply) {
                    if (!err) {
                        User.update(
                            { total_score: reply },
                            { where: { user_id: userId } }
                        ).then(function (reply) {
                            res.send({ error: 0, data: reply });
                        }).catch(function (error) {
                            res.send({ error: 2, data: error });
                        });
                    }
                    else {
                        res.send({error: 3, data: err});
                        return;
                    }
                })
            } else {
                res.send({ error: 0, data: ''});
            }
        } else {
            res.send({ error: 3, data: err });
        }
    });

});

// 유저 현재 순위 얻기
app.post('/users/:userId/rank', function (req, res) {
    req.assert('userId').notEmpty();
    req.checkBody('gameName').notEmpty();
    var errors = req.validationErrors();
    if (errors) {
        res.send({ error: 1, data: errors });
        return;
    }

    var userId = req.params.userId;
    var gameName = req.body.gameName;

    redisClient.zrevrank(gameName + '_rank', userId, function (err, reply) {
        if (!err) {
            if (reply != null)
                res.send({ error: 0, data: reply.toString() });
            else
                res.send({ error: 0, data: '' });
        }
        else {
            res.send({ error: 3, data: err });
        }
    });
});

// 전체 순위 정보 얻기 [1~10]
app.get('/leaderboard/:gameName', function (req, res) {
    req.assert('gameName').notEmpty();
    var errors = req.validationErrors();
    if (errors) {
        res.send({ error: 1, data: errors });
        return;
    }

    var gameName = req.params.gameName;

    redisClient.zrevrange(gameName + '_rank', 0, 9, 'withscores', function (err, reply) {
        if (!err) {
            var data = [];
            for (var i = 0, rank = 1; i < reply.length; i += 2, rank++) {
                data.push({ rank: rank, userId: reply[i], score: reply[i + 1] });
            }
            res.send({ error: 0, data: JSON.stringify(data) });
        }
        else {
            res.send({ error: 3, data: err });
        }
    });
});

// 아이템 구입하기
app.post('/users/:userId/buy/item', function (req, res) {
    req.assert('userId').notEmpty();
    req.checkBody('itemId').notEmpty();
    req.checkBody('point').notEmpty();
    req.checkBody('itemCost').notEmpty();
    var errors = req.validationErrors();
    if (errors) {
        res.send({ error: 1, data: errors });
        return;
    }

    var userId = req.params.userId;
    var itemId = req.body.itemId;
    var point = req.body.point;
    var itemCost = req.body.itemCost;

    UserItem.upsert(
        { user_id: userId, item_id: itemId,  point: point },
        { where: { user_id: userId, item_id: itemId } }
    ).then(function (test) {
        //res.send({ error: '', data: test});
    }).catch(function (error) {
        res.send({ error: 2, data: error });
    });

    User.update(
        { star_point: sequelize.literal('star_point -' + itemCost) },
        { where: { user_id: userId } }
    ).then(function (reply) {
        res.send({ error: 0, data: reply });
    }).catch(function (error) {
        res.send({ error: 2, data: error });
    });
});

// 아이템 사용
app.post('/users/:userId/use/item', function (req, res) {
    req.assert('userId').notEmpty();
    req.checkBody('itemId').notEmpty();
    var errors = req.validationErrors();
    if (errors) {
        res.send({ error: 1, data: errors });
        return;
    }

    var userId = req.params.userId;
    var itemId = req.body.itemId;

    UserItem.update(
        { user_id: userId, item_id: itemId,  point: sequelize.literal('point -1') },
        { where: { user_id: userId, item_id: itemId } }
    ).then(function (reply) {
        res.send({ error: 0, data: reply });
    }).catch(function (error) {
        res.send({ error: 2, data: error });
    });

});

// 사용자 등록
app.post('/users/:userId/join', function (req, res) {
    req.assert('userId').notEmpty();
    req.checkBody('password').notEmpty();
    req.checkBody('name').notEmpty();
    req.checkBody('email').notEmpty();
    var errors = req.validationErrors();
    if (errors) {
        res.send({ error: 1, data: errors });
        return;
    }

    var userId = req.params.userId;
    var password = req.body.password;
    var name = req.body.name;
    var email = req.body.email;

    User.create({
        user_id : userId,
        password : password,
        name : name,
        email : email,
        total_score : 0,
        star_point : 0
    }).then(function (userInfo) {
        res.send({ error: 0, data: JSON.stringify(userInfo) });
    }).catch(function (error) {
        res.send({ error: 2, data: error });
    });
});

// 로그인 [USER_INFO]
app.post('/users/:userId/login', function (req, res) {
    req.assert('userId').notEmpty();
    req.checkBody('password').notEmpty();
    var errors = req.validationErrors();
    if (errors) {
        res.send({ error: 1, data: errors });
        return;
    }

    var userId = req.params.userId;
    var password = req.body.password;

    User.find({ 
		where: sequelize.and(
			sequelize.where(
				sequelize.col('user_id'), sequelize.cast(userId, 'binary')
			),
			sequelize.where(			
				sequelize.col('password'), sequelize.cast(password, 'binary')
			)
		)
	}).then(function (userInfo) {
        if (userInfo) {
            res.send({ error: 0, data: JSON.stringify(userInfo) });
        } else
            res.send({ error: 4, data: '' });
    }).catch(function (error) {
        res.send({ error: 2, data: error });
    });
});

// 유저 게임점수 [USER_SCORE]
app.get('/users/:userId/scores', function (req, res) {
    req.assert('userId').notEmpty();
    var errors = req.validationErrors();
    if (errors) {
        res.send({ error: 1, data: errors });
        return;
    }

    var userId = req.params.userId;

    UserScore.findAll({ where: { user_id: userId } }).then(function (userScore) {
        if (userScore) {
            res.send({ error: 0, data: JSON.stringify(userScore) });
        } else
            res.send({ error: 0, data: '' });
    }).catch(function (error) {
        res.send({ error: 2, data: error });
    });
});

// 유저 아이템정보 [USER_SCORE]
app.get('/users/:userId/items', function (req, res) {
    req.assert('userId').notEmpty();
    var errors = req.validationErrors();
    if (errors) {
        res.send({ error: 1, data: errors });
        return;
    }

    var userId = req.params.userId;

    UserItem.findAll({ where: { user_id: userId } }).then(function (userItem) {
        if (userItem) {
            res.send({ error: 0, data: JSON.stringify(userItem) });
        } else
            res.send({ error: 0, data: '' });
    }).catch(function (error) {
        res.send({ error: 2, data: error });
    });
});

// 별 획득 [USER_INFO] [UPDATE] [POST]
app.post('/users/:userId/add/starPoint', function (req, res) {
    req.assert('userId').notEmpty();
    req.checkBody('point').notEmpty();
    var errors = req.validationErrors();
    if (errors) {
        res.send({ error: 1, data: errors });
        return;
    }

    var userId = req.params.userId;
    var point = req.body.point;

    User.update(
        { star_point: sequelize.literal('star_point +' + point) },
        { where: { user_id: userId } }
    ).then(function (reply) {
        res.send({ error: 0, data: reply });
    }).catch(function (error) {
        res.send({ error: 2, data: error });
    });
});


server.listen(80);