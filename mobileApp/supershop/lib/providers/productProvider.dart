import 'package:flutter/cupertino.dart';
import 'package:supershop/contracts/product_service.contract.dart';
import 'package:supershop/models/product.model.dart';

class ProductProvider with ChangeNotifier {
  ProductServiceContract _productContract;
  ProductProvider(this._productContract);

  Future<bool> addToCart(Product productToAdd) async {
    try {
      bool resp = await _productContract.addToCart(productToAdd);
      return resp;
    } catch (e) {
      throw e;
    }
  }

  Future<List<Product>>getCart()async{
    List<Product> list = await _productContract.getCart();
    return list;
  }
}
