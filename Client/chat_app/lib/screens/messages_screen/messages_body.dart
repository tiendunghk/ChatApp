import 'package:chat_app/models/chart_message.dart';
import 'package:chat_app/providers/message_provider.dart';
import 'package:chat_app/screens/messages_screen/chat_input/chat_input.dart';
import 'package:chat_app/screens/messages_screen/message.dart/message.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class MessagesBody extends StatefulWidget {
  final String groupId;
  const MessagesBody({Key? key, required this.groupId}) : super(key: key);

  @override
  _MessagesBodyState createState() => _MessagesBodyState();
}

class _MessagesBodyState extends State<MessagesBody> {
  var init = false;
  @override
  void didChangeDependencies() {
    if (!init) {
      Provider.of<MessageProvider>(context).getMessages(widget.groupId, 0);
      init = true;
    }
    super.didChangeDependencies();
  }

  @override
  Widget build(BuildContext context) {
    final messages = Provider.of<MessageProvider>(context).messages;
    print('groupid la: ${widget.groupId}');
    return Column(
      children: [
        messages.length <= 0
            ? Expanded(child: Center(child: Text('No messages!')))
            : Expanded(
                child: Padding(
                    padding: EdgeInsets.symmetric(horizontal: 8),
                    child: ListView.builder(
                        itemBuilder: (_, index) {
                          return Message(message: messages[index]);
                        },
                        itemCount: messages.length))),
        ChatInput(),
      ],
    );
  }
}
