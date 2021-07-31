import 'package:dio/dio.dart';
import 'package:shared_preferences/shared_preferences.dart';

class MyApiHelper {
  static final String url = 'http://10.0.3.2:9999/api/';
  static BaseOptions opts = BaseOptions(
    baseUrl: url,
    responseType: ResponseType.json,
  );

  static Dio dio = Dio(opts);

  MyApiHelper() {
    dio.interceptors.add(InterceptorsWrapper(
      onRequest: (options, handler) async {
        SharedPreferences prefs = await SharedPreferences.getInstance();
        final accesstoken = prefs.getString('access_token');
        print('zzzzz $accesstoken');
        if (accesstoken != null) {
          options.headers.addAll({"Authorization": "Bearer $accesstoken"});
        }
        handler.next(options);
      },
      onResponse: (response, handler) {
        print('phan hoi $response');
        handler.next(response);
      },
      onError: (DioError e, handler) {
        print('loi $e');
        handler.reject(e);
      },
    ));
  }

  Future<Response?> getHTTP(String url) async {
    try {
      print('get');
      Response response = await dio.get(url);
      return response;
    } on Exception catch (e) {
      // Handle error
      print('Looix $e');
    }
  }

  Future<Response?> postHTTP(String url, dynamic data) async {
    try {
      Response response = await dio.post(url, data: data);
      return response;
    } on DioError catch (e) {
      // Handle error
      print('Looix $e');
    }
  }
}
