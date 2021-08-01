class ApiResponse {
  bool? succeeded = false;
  String? message;
  List<String>? errors;
  String? errorCode;
  dynamic data;

  ApiResponse(
      {this.succeeded, this.message, this.errors, this.errorCode, this.data});
  factory ApiResponse.fromJson(dynamic json) => ApiResponse(
      succeeded: json['succeeded'] as bool?,
      message: json['message'] as String?,
      errors: json['errors'] as List<String>?,
      errorCode: json['errorCode'] as String?,
      data: json['data']);
}
