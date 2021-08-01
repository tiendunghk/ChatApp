import 'package:shared_preferences/shared_preferences.dart';
import 'package:signalr_core/signalr_core.dart';

class ChatHub {
  static final ChatHub _chatHub = ChatHub._internal();

  final connection = HubConnectionBuilder()
      .withUrl(
          'http://10.0.3.2:9999/hubChat',
          HttpConnectionOptions(
              logging: (level, message) => print(message),
              accessTokenFactory: () async {
                SharedPreferences sharedPred =
                    await SharedPreferences.getInstance();

                return sharedPred.getString("access_token");
              },
              withCredentials: false))
      .withAutomaticReconnect()
      .build();

  factory ChatHub() {
    return _chatHub;
  }

  ChatHub._internal();

  void connect() async {
    if (connection.state != HubConnectionState.connected) {
      await connection.start();

      connection.on("NhanMessage", (newMessage) {
        print('nhan tin nhan moi $newMessage');
        print(newMessage![0]['message']);
      });
    }
  }

  void disconnect() async {
    await connection.stop();
  }
}
