import 'package:chat_app/providers/group_chat_provider.dart';
import 'package:chat_app/screens/list_chat_screen/list_chat_body.dart';
import 'package:chat_app/signalr/chatHub.dart';
import 'package:chat_app/widgets/search/search_appbar_delegate.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class ListChatScreen extends StatefulWidget {
  static const routeName = "/chat-list";
  const ListChatScreen({Key? key}) : super(key: key);

  @override
  _ListChatScreenState createState() => _ListChatScreenState();
}

class _ListChatScreenState extends State<ListChatScreen> {
  late SearchAppBarDelegate _searchDelegate;
  @override
  void initState() {
    super.initState();
    print('intit');
    connectSignalR();

    _searchDelegate = SearchAppBarDelegate();
  }

  void connectSignalR() async {
    ChatHub().connect();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: buildAppBar(),
      body: ListChatBody(),
    );
  }

  AppBar buildAppBar() {
    return AppBar(
      title: Text("Chats"),
      automaticallyImplyLeading: false,
      actions: [
        IconButton(
            onPressed: () {
              ChatHub().disconnect();
              Navigator.of(context).pushReplacementNamed('/');
            },
            icon: Icon(Icons.logout)),
        IconButton(
          onPressed: () {
            _showSearchPage(context, _searchDelegate);
          },
          icon: Icon(Icons.search),
        )
      ],
    );
  }

  void _showSearchPage(
      BuildContext context, SearchAppBarDelegate searchDelegate) async {
    final String? selected = await showSearch<String>(
      context: context,
      delegate: searchDelegate,
    );

    print(selected);
  }
}
