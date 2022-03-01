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
      body: SingleChildScrollView(
        // controller: controller,
        child: Column(
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
            FutureBuilder(
              future: _cartItems,
              builder: (context, AsyncSnapshot<List<Product>> snapshot) {
                if (snapshot.hasError) {
                  return Text('error');
                }
                if (snapshot.connectionState == ConnectionState.done) {
                  if (snapshot.hasData) {
                    // return Container(
                    //     child: ListView.builder(
                    //         itemCount: snapshot.data.length,
                    //         scrollDirection: Axis.horizontal,
                    //         itemBuilder: (BuildContext context, int index) {
                    //           return Text('${snapshot.data[index].name}');
                    //         }));
                  } else {
                    return Text('No products yet');
                  }
                } else {
                  return CircularProgressIndicator();
                }
                return Text('');
              },
            ),
            CartItem(
              productName: "Zapato rojo",
              productPrice: 800,
              productImage:
                  "https://media.revistagq.com/photos/5ca5e52ec57c5b6f74c53c2c/16:9/w_2580,c_limit/tipos_corte_camisa_hombre_regular_fit_slim_fit_110345264.jpg",
            ),
            CartItem(
              productName: "Zapato rojo",
              productPrice: 800,
              productImage:
                  "https://media.revistagq.com/photos/5ca5e52ec57c5b6f74c53c2c/16:9/w_2580,c_limit/tipos_corte_camisa_hombre_regular_fit_slim_fit_110345264.jpg",
            ),
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
      ),
    );
  }
}
