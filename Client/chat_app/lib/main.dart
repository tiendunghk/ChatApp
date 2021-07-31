import 'package:chat_app/screens/list_chat_screen/list_chat_screen.dart';
import 'package:chat_app/screens/login/login.dart';
import 'package:chat_app/theme.dart';
import 'package:flutter/material.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: darkThemeData(context),
      routes: {
        "/": (_) => Login(),
        "/list-chat": (_) => ListChatScreen(),
      },
    );
  }
}
