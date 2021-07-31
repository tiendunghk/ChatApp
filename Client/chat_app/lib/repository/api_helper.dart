import 'package:dio/dio.dart';
import 'package:shared_preferences/shared_preferences.dart';

class ApiHelper {
  static final String url = 'http://10.0.3.2:9999/api/';
  static BaseOptions opts = BaseOptions(
      baseUrl: url,
      responseType: ResponseType.json,
      validateStatus: (valStatus) {
        return valStatus! <= 500;
      });

  static Dio createDio() {
    return Dio(opts);
  }

  static Dio addInterceptors(Dio dio) {
    return dio
      ..interceptors.add(
        InterceptorsWrapper(onRequest: (RequestOptions options, _) {
          requestInterceptor(options);
          _.next(options);
        }, onError: (DioError e, _) async {
          print('loi $e');
          _.reject(e);
        }, onResponse: (response, handler) {
          print('phan hoi la : $response');
        }),
      );
  }

  static dynamic requestInterceptor(RequestOptions options) async {
    // Get your JWT token
    SharedPreferences prefs = await SharedPreferences.getInstance();
    var token = prefs.getString('access_token');
    print('token laf: $token');
    if (token != null) {
      options.headers.addAll({"Authorization": "Bearer: $token"});
    }
    return options;
  }

  static final dio = createDio();
  static final baseAPI = addInterceptors(dio);

  Future<Response?> getHTTP(String url) async {
    try {
      print('get');
      Response response = await baseAPI.get(url);
      return response;
    } on Exception catch (e) {
      // Handle error
      print('Looix $e');
    }
  }

  Future<Response?> postHTTP(String url, dynamic data) async {
    try {
      Response response = await baseAPI.post(url, data: data);
      return response;
    } on DioError catch (e) {
      // Handle error
      print('Looix $e');
    }
  }

  Future<Response?> putHTTP(String url, dynamic data) async {
    try {
      Response response = await baseAPI.put(url, data: data);
      return response;
    } on DioError catch (e) {
      // Handle error
    }
  }

  Future<Response?> deleteHTTP(String url) async {
    try {
      Response response = await baseAPI.delete(url);
      return response;
    } on DioError catch (e) {
      // Handle error
    }
  }
}
