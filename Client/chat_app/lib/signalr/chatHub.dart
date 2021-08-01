import 'package:shared_preferences/shared_preferences.dart';
import 'package:signalr_core/signalr_core.dart';

class ChatHub {
  static final ChatHub _chatHub = ChatHub._internal();

  factory ChatHub() {
    return _chatHub;
  }

  ChatHub._internal();

  void connectHub() async {
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

    await connection.start();

    connection.on("NhanMessage", (newMessage) {
      print('nhan tin nhan moi $newMessage');
      print(newMessage![0]['message']);
    });
  }
}
