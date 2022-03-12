import 'package:supershop/models/Address.model.dart';
import 'package:supershop/models/branch.model.dart';
import 'package:supershop/models/mall.model.dart';
import 'package:supershop/models/product.model.dart';

abstract class ProductServiceContract {
  Future<bool>addToCart(Product productToAdd);
  Future<List<Product>>getCart();
  Future<bool>deleteFromCart(int productId);
  Future<bool>addToAddress(Address address);
  Future<List<Address>>getAddresses();
  Future<bool>deleteFromAddresses(String addressAlias);
  Future<List<Mall>>getAllMalls();
  Future<List<Branch>>getStores(Mall mall);
  Future<List<Branch>>getAllStores();
  Future<String>getMallNameFromId(String mallId);
  Future<List<Product>>getProductsByStore(Branch store);
}