class UserSearchResponse {
  String userId;
  String userFullname;
  String userImageUrl;

  UserSearchResponse({
    required this.userId,
    required this.userFullname,
    required this.userImageUrl,
  });

  factory UserSearchResponse.fromJson(Map<String, dynamic> json) =>
      UserSearchResponse(
        userId: json['userId'],
        userFullname: json['userFullname'],
        userImageUrl: json['userImageUrl'],
      );
}
