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
  var _scrollController = ScrollController();
  @override
  void didChangeDependencies() async {
    if (!init) {
      await Provider.of<MessageProvider>(context, listen: false)
          .getMessages(widget.groupId, true);

      final mes =
          Provider.of<MessageProvider>(context, listen: false).messages.length;
      if (mes != 0) scrollBottom();
      init = true;
    }
    super.didChangeDependencies();
  }

  @override
  void initState() {
    super.initState();

    _scrollController.addListener(() {
      if (_scrollController.position.atEdge) {
        if (_scrollController.position.pixels == 0) {
          print('reach top');
          Provider.of<MessageProvider>(context, listen: false)
              .getMessages(widget.groupId, false);
        } else {
          // You're at the bottom.
        }
      }
    });
  }

  @override
  void dispose() {
    super.dispose();
    _scrollController.dispose();
  }

  void scrollBottom() {
    _scrollController.animateTo(
      _scrollController.position.maxScrollExtent,
      duration: Duration(seconds: 1),
      curve: Curves.fastOutSlowIn,
    );
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
                        controller: _scrollController,
                        itemBuilder: (_, index) {
                          return Message(message: messages[index]);
                        },
                        itemCount: messages.length))),
        ChatInput(),
      ],
    );
  }
}
