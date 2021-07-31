class UserResponse {
  String id;
  String email;
  String jwToken;
  String refreshToken;
  String fullName;
  String userAvatar;

  UserResponse(
      {required this.id,
      required this.email,
      required this.jwToken,
      required this.refreshToken,
      required this.fullName,
      required this.userAvatar});

  factory UserResponse.fromJson(Map<String, dynamic> json) => UserResponse(
      id: json['id'],
      email: json['email'],
      jwToken: json['jwToken'],
      refreshToken: json['refreshToken'],
      fullName: json['fullName'],
      userAvatar: json['userAvatar']);
}
