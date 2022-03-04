import 'package:flutter/cupertino.dart';
import 'package:supershop/contracts/product_service.contract.dart';
import 'package:supershop/models/malls.model.dart';
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

    Future<bool> deleteCart(int productId) async {
    try {
      bool resp = await _productContract.deleteFromCart(productId);
      return resp;
    } catch (e) {
      throw e;
    }
  }

  Future<List<Malls>>getMalls()async{
    try {
      List<Malls> malls = await _productContract.getAllMalls();
      return malls;
    } catch (e) {
      throw e;
    }
  }

  Future<List<Product>>getCart()async{
    List<Product> list = await _productContract.getCart();
    return list;
  }
}
