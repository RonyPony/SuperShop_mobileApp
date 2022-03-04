import 'dart:convert';

import 'package:shared_preferences/shared_preferences.dart';
import 'package:supershop/contracts/product_service.contract.dart';
import 'package:supershop/helpers/requestsManager.dart';
import 'package:supershop/models/loginResponse.model.dart';
import 'package:supershop/models/malls.model.dart';
import 'package:supershop/models/product.model.dart';
import 'package:supershop/widgets/cartItem.dart';

class ProductService implements ProductServiceContract {
  String SAVED_PRODUCT_KEY = "ProductosdeSuperShop";
  List<Product> _cartItems = new List<Product>();

  @override
  Future<bool> addToCart(Product productToAdd) async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    _cartItems.add(productToAdd);
    String data = json.encode(_cartItems);
    bool saved = await prefs.setString(SAVED_PRODUCT_KEY, data);
    return saved;
  }

  @override
  Future<List<Product>> getCart() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    String jsonedCart = prefs.getString(SAVED_PRODUCT_KEY);
    if (jsonedCart != null) {
      List<dynamic> parsedListJson = jsonDecode(jsonedCart);
      List<Product> itemsList =
          List<Product>.from(parsedListJson.map((i) => Product.fromJson(i)));
      return itemsList;
    } else {
      return null;
    }
  }

  @override
  Future<bool> deleteFromCart(int productId) async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    String jsonedCart = prefs.getString(SAVED_PRODUCT_KEY);
    if (jsonedCart != null) {
      List<dynamic> parsedListJson = jsonDecode(jsonedCart);
      List<Product> itemsList =
          List<Product>.from(parsedListJson.map((i) => Product.fromJson(i)));
      final index = itemsList.indexWhere((element) => element.id == productId);
      itemsList.removeAt(index);
      String data = json.encode(itemsList);
      bool saved = await prefs.setString(SAVED_PRODUCT_KEY, data);
      return saved;
    } else {
      return null;
    }
  }

  @override
  Future<List<Malls>> getAllMalls() async {
    final client = RequestsManager.requester();
      final response = await client.get("/Mall/All",);
      if (response.statusCode < 400) {
        
        List<Malls> mallsList;
         Malls xx= Malls.fromJson(response.data);
        
        return mallsList;
      } else {
        //TODO
      }
  }
}
