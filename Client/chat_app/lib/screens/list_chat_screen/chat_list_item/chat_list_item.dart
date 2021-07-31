import 'package:chat_app/constants.dart';
import 'package:chat_app/models/chat.dart';
import 'package:chat_app/screens/messages_screen/messages_screen.dart';
import 'package:flutter/material.dart';

class ChatListItem extends StatelessWidget {
  final Chat chat;
  const ChatListItem({Key? key, required this.chat}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return InkWell(
      onTap: () {
        Navigator.of(context)
            .push(MaterialPageRoute(builder: (_) => MessagesScreen()));
      },
      child: Padding(
        padding: const EdgeInsets.symmetric(
            horizontal: kDefaultPadding, vertical: kDefaultPadding * 0.75),
        child: Row(
          children: [
            CircleAvatar(
              backgroundImage: AssetImage(chat.image),
            ),
            Expanded(
                child: Padding(
              padding: const EdgeInsets.symmetric(horizontal: kDefaultPadding),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    chat.name,
                    style: TextStyle(fontSize: 15, fontWeight: FontWeight.w500),
                  ),
                  SizedBox(
                    height: 8,
                  ),
                  Opacity(
                    opacity: 0.64,
                    child: Text(
                      chat.lastMessage,
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                    ),
                  )
                ],
              ),
            )),
            Opacity(
              opacity: 0.64,
              child: Text(chat.time),
            )
          ],
        ),
      ),
    );
  }
}
