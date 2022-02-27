import 'dart:convert';

import 'package:shared_preferences/shared_preferences.dart';
import 'package:supershop/contracts/product_service.contract.dart';
import 'package:supershop/models/product.model.dart';

class ProductService implements ProductServiceContract {
  String SAVED_PRODUCT_KEY = "ProductosdeSuperShop";


  @override
  Future<bool> addToCart(Product productToAdd)async{
    SharedPreferences prefs = await SharedPreferences.getInstance();
    String data = json.encode(productToAdd.toJson());
    bool saved = await prefs.setString(SAVED_PRODUCT_KEY,data );
    return saved;
  }

  @override
  Future<List<Product>> getCart()async{
    SharedPreferences prefs = await SharedPreferences.getInstance();
    String saved =  prefs.getString(SAVED_PRODUCT_KEY );
   List<Product> jsonResponse= json.decode(saved);
   //TODO finish this
  
  }
  
}