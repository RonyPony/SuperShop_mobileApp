import 'package:dio/dio.dart';
import 'package:supershop/constants.dart';

class RequestsManager {
  static Dio requester() {
    final Dio returningDio = createRequester();
    returningDio.options.followRedirects = true;
    returningDio.options.validateStatus = (status) {
      return status < 500;
    };
    returningDio.interceptors
        .add(LogInterceptor(requestBody: true, responseBody: true));
    return returningDio;
  }
  

  static Dio createRequester() {
    String apiURL = authApiUrl;
    Dio dio = Dio();
    dio.options.baseUrl = '$apiURL/api/';
    dio.options.followRedirects = false;
    dio.options.connectTimeout = 20 * 3000;
    dio.options.receiveTimeout = 30 * 3000;
    return dio;
  }
}
