class ChatMessage {
  final String text;
  final bool isSender;
  final DateTime sendedAt;

  ChatMessage({
    this.text = '',
    required this.sendedAt,
    required this.isSender,
  });
}

List demoChatMessages = [
  ChatMessage(
    text: "Hi Sajol,",
    sendedAt: DateTime.now().add(Duration(minutes: -10)),
    isSender: false,
  ),
  ChatMessage(
    text: "Hello, How are you?",
    sendedAt: DateTime.now().add(Duration(minutes: -9)),
    isSender: true,
  ),
  ChatMessage(
    text: "ccadsd",
    sendedAt: DateTime.now().add(Duration(minutes: -8)),
    isSender: false,
  ),
  ChatMessage(
    text: "ccccccc",
    sendedAt: DateTime.now().add(Duration(minutes: -7)),
    isSender: true,
  ),
  ChatMessage(
    text: "Error happend",
    sendedAt: DateTime.now().add(Duration(minutes: -6)),
    isSender: true,
  ),
  ChatMessage(
    text: "This looks great man!!",
    sendedAt: DateTime.now().add(Duration(minutes: -5)),
    isSender: false,
  ),
  ChatMessage(
    text: "Glad you like it",
    sendedAt: DateTime.now().add(Duration(minutes: -4)),
    isSender: true,
  ),
];
