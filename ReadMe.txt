이 어플리케이션은 네트워크 오프라인 환경에서 실행되지않습니다
HTTP통신이 가능한 서버와 DB가 필요하며 서버는 파일을 참고하여 직접 새로 만드셔도 되고
아니면 해당소스를 복사하여 NodeJs - NPM 을 설치한후 웹서버를 여셔야 합니다.
DB는 MySQL을 사용하였으며 다른 DB 엔진을 사용하실경우 서버파일의 app.js부분을 수정하셔야 합니다.
DB 뿐만아니라 랭킹을 위해 Redis서버를 별도로 사용하였으며 이것또한 app.js부분을 참고하시길 바랍니다.

어플리케이션의 경우 Assets/Common/Script/WWWManager.cs 클래스의 serverIp 멤버변수에
유효한 서버주소를 넣어주셔야 합니다.
-made by tkqlzz