import 'package:flutter/cupertino.dart';
import 'package:supershop/contracts/product_service.contract.dart';
import 'package:supershop/models/Address.model.dart';
import 'package:supershop/models/branch.model.dart';
import 'package:supershop/models/mall.model.dart';
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

  Future<bool>cleanCart() async {
    bool resp = await _productContract.cleanCart();
    return resp;
  }

  Future<bool> addToAddress(Address address) async {
    try {
      bool resp = await _productContract.addToAddress(address);
      return resp;
    } catch (e) {
      throw e;
    }
  }

    Future<bool> deleteCart(String productId) async {
    try {
      bool resp = await _productContract.deleteFromCart(productId);
      return resp;
    } catch (e) {
      throw e;
    }
  }


   Future<bool> deleteAddress(String addressAlias) async {
    try {
      bool resp = await _productContract.deleteFromAddresses(addressAlias);
      return resp;
    } catch (e) {
      throw e;
    }
  }

  Future<List<Branch>>getMallStores(Mall mall) async {
try {
  List<Branch> lista = await _productContract.getStores(mall);
  return lista;
} catch (e) {
  throw e;
}
  }

  Future<List<Mall>>getMalls()async{
    try {
      List<Mall> malls = await _productContract.getAllMalls();
      return malls;
    } catch (e) {
      throw e;
    }
  }
  Future<List<Address>>getAddresses()async{
    List<Address> list = await _productContract.getAddresses();
    return list;
  }


  Future<List<Branch>>getAllStores()async{
    List<Branch> list = await _productContract.getAllStores();
    return list;
  }

  Future<List<Product>>getCart()async{
    List<Product> list = await _productContract.getCart();
    return list;
  }

  Future<String> getMallNameFromMallId(String mallId)async{
    String list = await _productContract.getMallNameFromId(mallId);
    return list;
  }

  Future<List<Product>> getProductsFromStore(Branch store)async{
    List<Product> prods = await _productContract.getProductsByStore(store);
    return prods;
  }

  Future<String> getCategoryName(String storeId) {
    Future<String> resp = _productContract.getCategory(storeId);
    return resp;
  }
}
