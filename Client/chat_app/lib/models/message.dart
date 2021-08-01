class Message {
  String messageId;
  String messageUserId;
  String messageGroupChatId;
  String messageContent;
  DateTime messageCreatedAt;
  String messengerUserAvatar;
  String messengerUserName;

  Message(
      {required this.messageId,
      required this.messageUserId,
      required this.messageGroupChatId,
      required this.messageContent,
      required this.messageCreatedAt,
      required this.messengerUserAvatar,
      required this.messengerUserName});

  factory Message.fromJson(Map<String, dynamic> json) => Message(
      messageId: json['messageId'],
      messageUserId: json['messageUserId'],
      messageGroupChatId: json['messageGroupChatId'],
      messageContent: json['messageContent'],
      messageCreatedAt: DateTime.parse(json['messageCreatedAt']),
      messengerUserAvatar: json['messengerUserAvatar'],
      messengerUserName: json['messengerUserName']);
}
