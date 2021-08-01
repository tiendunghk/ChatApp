import 'package:chat_app/constants.dart';
import 'package:chat_app/models/chat.dart';
import 'package:chat_app/providers/group_chat_provider.dart';
import 'package:chat_app/screens/messages_screen/messages_screen.dart';
import 'package:flutter/material.dart';
import 'package:timeago/timeago.dart' as timeago;

class ChatListItem extends StatelessWidget {
  final GroupChatModel chat;
  const ChatListItem({Key? key, required this.chat}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return InkWell(
      onTap: () {
        Navigator.of(context).pushNamed(MessagesScreen.routeName,arguments: chat);
      },
      child: Padding(
        padding: const EdgeInsets.symmetric(
            horizontal: defaultPadding, vertical: defaultPadding * 0.75),
        child: Row(
          children: [
            CircleAvatar(
              backgroundImage: NetworkImage(chat.groupAvatar),
            ),
            Expanded(
                child: Padding(
              padding: const EdgeInsets.symmetric(horizontal: defaultPadding),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    chat.groupChatName,
                    style: TextStyle(fontSize: 15, fontWeight: FontWeight.w500),
                  ),
                  SizedBox(
                    height: 8,
                  ),
                  chat.lastestMes != null
                      ? Opacity(
                          opacity: 0.64,
                          child: Text(
                            chat.lastestMes!,
                            maxLines: 1,
                            overflow: TextOverflow.ellipsis,
                          ),
                        )
                      : Container()
                ],
              ),
            )),
            Opacity(
              opacity: 0.64,
              child: Text(
                  timeago.format(chat.groupChatUpdatedAt, locale: 'en_short')),
            )
          ],
        ),
      ),
    );
  }
}
