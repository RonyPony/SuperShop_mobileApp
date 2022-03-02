import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:provider/provider.dart';
import 'package:supershop/models/product.model.dart';
import 'package:supershop/providers/productProvider.dart';
import 'package:supershop/screens/home.screen.dart';
import 'package:supershop/widgets/cartItem.dart';
import 'package:supershop/widgets/sideMenuDrawer.dart';

class CartScreen extends StatefulWidget {
  CartScreen({Key key}) : super(key: key);
  static String routeName = "/cartScreen";

  @override
  State<CartScreen> createState() => _CartScreenState();
}

class _CartScreenState extends State<CartScreen> {
  @override
  Widget build(BuildContext context) {
    final _productProvider =
        Provider.of<ProductProvider>(context, listen: false);
    Future<List<Product>> _cartItems = _productProvider.getCart();
    return Scaffold(
      drawer: SideMenuDrawer(),
      appBar: AppBar(),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(30.0),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                SvgPicture.asset(
                  "assets/carrito-negro.svg",
                  width: 30,
                ),
                Text(
                  "Carrito",
                  style: TextStyle(fontSize: 25, fontWeight: FontWeight.bold),
                )
              ],
            ),
          ),
          _buildProducts(_cartItems),
          Padding(
            padding: const EdgeInsets.only(right: 50, top: 15),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.end,
              children: [
                Text(
                  'Total:',
                  style: TextStyle(fontWeight: FontWeight.bold, fontSize: 20),
                ),
                Text(
                  'RD 1000',
                  style: TextStyle(fontSize: 18),
                )
              ],
            ),
          ),
          Padding(
            padding: const EdgeInsets.only(top: 30),
            child: ElevatedButton(
              style: ButtonStyle(
                  backgroundColor: MaterialStateProperty.all(Colors.white),
                  shape: MaterialStateProperty.all<RoundedRectangleBorder>(
                      RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(50),
                          side: BorderSide(color: Colors.black)))),
              child: Container(
                  width: 150,
                  child: Center(
                    child: Text(
                      'Comprar todo',
                      style: TextStyle(color: Colors.blue, fontSize: 17),
                    ),
                  )),
              onPressed: () {
                // print(nameController.text);
                // print(passwordController.text);
              },
            ),
          ),
          ElevatedButton(
            style: ButtonStyle(
                backgroundColor: MaterialStateProperty.all(Colors.blue),
                shape: MaterialStateProperty.all<RoundedRectangleBorder>(
                    RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(50),
                        side: BorderSide(color: Colors.black)))),
            child: Container(
                width: 150,
                child: Center(
                  child: Text(
                    'Regresar',
                    style: TextStyle(color: Colors.white, fontSize: 17),
                  ),
                )),
            onPressed: () {
              Navigator.pushNamed(context, HomeScreen.routeName);
            },
          ),
        ],
      ),
    );
  }

  _buildProducts(Future<List<Product>> futuro) {
    Size screenSize = MediaQuery.of(context).size;
    return FutureBuilder<List<Product>>(
      future: futuro,
      builder: (context, snapshot) {
        if (snapshot.hasData) {
          List<Product> orderItems = snapshot.data;
          ScrollController _scrollController = ScrollController();
if (snapshot.data.length == 0) {
                    return Text('No hay items en el carrito');
                  }
          return Container(
            height: screenSize.height * 0.5,
            child: Scrollbar(
              isAlwaysShown: true,
              radius: Radius.circular(50),
              thickness: 15,
              controller: _scrollController,
              child: ListView.builder(
                controller: _scrollController,
                shrinkWrap: true,
                itemCount: snapshot.data.length,
                itemBuilder: (context, index) {
                 
                    return CartItem(
                        productId: snapshot.data[index].id,
                        productName: snapshot.data[index].name,
                        productPrice: snapshot.data[index].price,
                        productImage: snapshot.data[index].imageUrl);
                  
                },
              ),
            ),
          );
        } else {
          if (snapshot.hasError) {
            return Text('loadingErrorShort');
          } else {
            return Center(child: CircularProgressIndicator());
          }
        }
      },
    );
  }
}
