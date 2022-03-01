import 'dart:convert';

import 'package:shared_preferences/shared_preferences.dart';
import 'package:supershop/contracts/product_service.contract.dart';
import 'package:supershop/models/product.model.dart';
import 'package:supershop/widgets/cartItem.dart';

class ProductService implements ProductServiceContract {
  String SAVED_PRODUCT_KEY = "ProductosdeSuperShop";
  List<Product> _cartItems = new List<Product>();

  @override
  Future<bool> addToCart(Product productToAdd)async{
    SharedPreferences prefs = await SharedPreferences.getInstance();
    _cartItems.add(productToAdd);
    String data = json.encode(_cartItems);
    bool saved = await prefs.setString(SAVED_PRODUCT_KEY,data );
    return saved;
  }

  @override
  Future<List<Product>> getCart()async{
    SharedPreferences prefs = await SharedPreferences.getInstance();
    String jsonedCart = prefs.getString(SAVED_PRODUCT_KEY);
    List<dynamic> parsedListJson = jsonDecode(jsonedCart);
    List<Product> itemsList = List<Product>.from(parsedListJson.map((i) => Product.fromJson(i)));
    return itemsList;
  }
  
}