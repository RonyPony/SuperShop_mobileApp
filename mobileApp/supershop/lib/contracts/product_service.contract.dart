import 'package:supershop/models/malls.model.dart';
import 'package:supershop/models/product.model.dart';

abstract class ProductServiceContract {
  Future<bool>addToCart(Product productToAdd);
  Future<List<Product>>getCart();
  Future<bool>deleteFromCart(int productId);
  Future<List<Malls>>getAllMalls();
}