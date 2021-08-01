import 'package:chat_app/providers/group_chat_provider.dart';
import 'package:chat_app/providers/user_provider.dart';
import 'package:chat_app/screens/list_chat_screen/list_chat_screen.dart';
import 'package:chat_app/screens/login/login.dart';
import 'package:chat_app/screens/messages_screen/messages_screen.dart';
import 'package:chat_app/theme.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MultiProvider(
      providers: [
        ChangeNotifierProvider(
          create: (_) => GroupChat(),
        ),
        ChangeNotifierProvider(
          create: (_) => UserProvider(),
        )
      ],
      child: MaterialApp(
        title: 'Flutter Demo',
        theme: darkThemeData(context),
        routes: {
          "/": (_) => Login(),
          ListChatScreen.routeName: (_) => ListChatScreen(),
          MessagesScreen.routeName: (_) => MessagesScreen(),
        },
      ),
    );
  }
}
