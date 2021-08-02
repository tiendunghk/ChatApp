class UserRequest {
  final String email;
  final String fullName;

  UserRequest({required this.email, required this.fullName});

  Map<String, dynamic> toJson() => {
        'email': this.email,
        'fullName': this.fullName,
      };
}
