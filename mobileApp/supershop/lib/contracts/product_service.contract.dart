import 'package:supershop/models/branch.model.dart';
import 'package:supershop/models/mall.model.dart';
import 'package:supershop/models/product.model.dart';

abstract class ProductServiceContract {
  Future<bool>addToCart(Product productToAdd);
  Future<List<Product>>getCart();
  Future<bool>deleteFromCart(int productId);
  Future<List<Mall>>getAllMalls();
  Future<List<Branch>>getStores(Mall mall);
}