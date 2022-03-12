import 'dart:convert';

import 'package:shared_preferences/shared_preferences.dart';
import 'package:supershop/contracts/product_service.contract.dart';
import 'package:supershop/helpers/requestsManager.dart';
import 'package:supershop/models/Address.model.dart';
import 'package:supershop/models/branch.model.dart';
import 'package:supershop/models/loginResponse.model.dart';
import 'package:supershop/models/mall.model.dart';
import 'package:supershop/models/product.model.dart';
import 'package:supershop/widgets/cartItem.dart';

class ProductService implements ProductServiceContract {
  String SAVED_PRODUCT_KEY = "ProductosdeSuperShop";
  String SAVED_Address_KEY = "DireccionesdeSuperShop";
  List<Product> _cartItems = new List<Product>();
  List<Address> _addresses = new List<Address>();

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
  Future<List<Mall>> getAllMalls() async {
    final client = RequestsManager.requester();
    final response = await client.get(
      "/Mall/All",
    );

    if (response.statusCode < 400) {
      List<dynamic> parsedListJson = response.data;
      List<Mall> mallsList;
      mallsList = List<Mall>.from(parsedListJson.map((i) => Mall.fromJson(i)));
      return mallsList;
    } else {
      //TODO
    }
  }

  @override
  Future<List<Branch>> getStores(Mall mall) async {
    final client = RequestsManager.requester();
    final response = await client.get(
      "/Branch/by-mall/${mall.id}",
    );

    if (response.statusCode < 400) {
      List<dynamic> parsedListJson = response.data;
      List<Branch> branchList;
      branchList =
          List<Branch>.from(parsedListJson.map((i) => Branch.fromJson(i)));
      return branchList;
    } else {
      //TODO
    }
  }

  @override
  Future<bool> addToAddress(Address address) async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    _addresses.add(address);
    String data = json.encode(_addresses);
    bool saved = await prefs.setString(SAVED_Address_KEY, data);
    return saved;
  }

  @override
  Future<bool> deleteFromAddresses(String addressAlias) async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    String jsonedAddresses = prefs.getString(SAVED_Address_KEY);
    if (jsonedAddresses != null) {
      List<dynamic> parsedListJson = jsonDecode(jsonedAddresses);
      List<Address> itemsList =
          List<Address>.from(parsedListJson.map((i) => Address.fromJson(i)));
      final index = itemsList
          .indexWhere((element) => element.addressAlias == addressAlias);
      itemsList.removeAt(index);
      String data = json.encode(itemsList);
      bool saved = await prefs.setString(SAVED_Address_KEY, data);
      return saved;
    } else {
      return null;
    }
  }

  @override
  Future<List<Address>> getAddresses() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    String jsonedAddresses = prefs.getString(SAVED_Address_KEY);
    if (jsonedAddresses != null) {
      List<dynamic> parsedListJson = jsonDecode(jsonedAddresses);
      List<Address> itemsList =
          List<Address>.from(parsedListJson.map((i) => Address.fromJson(i)));
      return itemsList;
    } else {
      return null;
    }
  }

  @override
  Future<List<Branch>> getAllStores() async {
    final client = RequestsManager.requester();
    final response = await client.get(
      "/Branch/All",
    );

    if (response.statusCode < 400) {
      List<dynamic> parsedListJson = response.data;
      List<Branch> branchList;
      branchList =
          List<Branch>.from(parsedListJson.map((i) => Branch.fromJson(i)));
      return branchList;
    } else {
      //TODO
    }
  }

  @override
  Future<String> getMallNameFromId(String mallId) async {
    final client = RequestsManager.requester();
    final response = await client.get(
      "/Mall/{$mallId}",
    );

    if (response.statusCode < 400) {
      dynamic parsedListJson = response.data;
      Mall mallsList;
      mallsList = Mall.fromJson(parsedListJson);
      return mallsList.name;
    } else {
      //TODO
    }
  }

  @override
  Future<List<Product>> getProductsByStore(Branch store) async {
    final client = RequestsManager.requester();
    final response = await client.get(
      "/Product/by-branch/${store.id}",
    );

    if (response.statusCode < 400) {
      List<dynamic> parsedListJson = response.data;
      List<Product> productList;
      productList =
          List<Product>.from(parsedListJson.map((i) => Product.fromJson(i)));
      return productList;
    } else {
      //TODO
    }
  }
}
