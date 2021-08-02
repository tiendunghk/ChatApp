import 'package:chat_app/providers/group_chat_provider.dart';
import 'package:chat_app/screens/list_chat_screen/chat_list_item/chat_list_item.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class ListChatBody extends StatelessWidget {
  const ListChatBody({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return FutureBuilder(
        future: Provider.of<GroupChatProvider>(context, listen: false).getGroupChat(),
        builder: (ctx, snapshot) =>
            snapshot.connectionState == ConnectionState.waiting
                ? Center(
                    child: CircularProgressIndicator(),
                  )
                : Consumer<GroupChatProvider>(
                    child: Center(
                      child: Text("No chat available!"),
                    ),
                    builder: (ctx, groupChat, child) {
                      if (groupChat.items.length <= 0) return child!;

                      print('length day: ${groupChat.items.length}');
                      
                      return Column(
                        children: [
                          Expanded(
                              child: ListView.builder(
                                  itemBuilder: (ctx, index) {
                                    return ChatListItem(
                                        chat: groupChat.items[index]);
                                  },
                                  itemCount: groupChat.items.length))
                        ],
                      );
                    }));
  }
}
