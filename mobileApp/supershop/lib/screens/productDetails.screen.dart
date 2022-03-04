import 'package:cool_alert/cool_alert.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:provider/provider.dart';
import 'package:supershop/models/product.model.dart';
import 'package:supershop/providers/productProvider.dart';
import 'package:supershop/screens/cart.screen.dart';

class ProductDetailsScreen extends StatefulWidget {
  ProductDetailsScreen({Key key}) : super(key: key);
  static String routeName = "/productScreen";
  @override
  State<ProductDetailsScreen> createState() => _ProductDetailsScreenState();
}

class _ProductDetailsScreenState extends State<ProductDetailsScreen> {
  @override
  Widget build(BuildContext context) {
    Size screenSize = MediaQuery.of(context).size;
    final productArgs = ModalRoute.of(context).settings.arguments as Product;

    return Scaffold(
      appBar: AppBar(
        title: Text("Detalles del producto"),
      ),
      body: SingleChildScrollView(
        // controller: controller,
        child: Padding(
          padding: const EdgeInsets.all(10.0),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Container(
                      height: screenSize.height * 0.6,
                      child: Image.network(productArgs.imageUrl)),
                ],
              ),
              Text(productArgs.name),
              Text("RD" + productArgs.price.toString()),
              SizedBox(
                height: 10,
              ),
              Text(
                "Detalles:",
                style: TextStyle(fontWeight: FontWeight.bold),
              ),
              Container(
                child: Text(
                    "Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500, cuando un impresor (N. del T. persona que se dedica a la imprenta) desconocido usó una galería de textos y los mezcló de tal manera que logró hacer un libro de textos especimen. No sólo sobrevivió 500 años, sino que tambien ingresó como texto de relleno en documentos electrónicos, quedando esencialmente igual al original. Fue popularizado en los 60s con la creación de las hojas , las cuales contenian pasajes de Lorem Ipsum, y más recientemente con software de autoedición, como por ejemplo Aldus PageMaker, el cual incluye versiones de Lorem Ipsum."),
              ),
              Center(
                child: Column(
                  children: [
                    ElevatedButton(
                      style: ButtonStyle(
                          backgroundColor:
                              MaterialStateProperty.all(Colors.blue),
                          shape:
                              MaterialStateProperty.all<RoundedRectangleBorder>(
                                  RoundedRectangleBorder(
                                      borderRadius: BorderRadius.circular(50),
                                      side: BorderSide(color: Colors.black)))),
                      child: Container(
                          width: 150,
                          child: Center(
                            child: Text('Comprar'),
                          )),
                      onPressed: () {
                        // print(nameController.text);
                        // print(passwordController.text);
                      },
                    ),
                    ElevatedButton(
                      style: ButtonStyle(
                          backgroundColor:
                              MaterialStateProperty.all(Colors.white),
                          shape:
                              MaterialStateProperty.all<RoundedRectangleBorder>(
                                  RoundedRectangleBorder(
                                      borderRadius: BorderRadius.circular(50),
                                      side: BorderSide(
                                          color: Colors.black, width: 5)))),
                      child: Container(
                          width: 150,
                          child: Center(
                              child: Row(
                            children: [
                              SvgPicture.asset(
                                "assets/carrito-azul.svg",
                                width: 30,
                              ),
                              SizedBox(
                                width: 10,
                              ),
                              Text(
                                'Agregar al carrito',
                                style: TextStyle(color: Colors.blue),
                              ),
                            ],
                          ))),
                      onPressed: () async {
                        final _productProvider = Provider.of<ProductProvider>(
                            context,
                            listen: false);
                        bool response =
                            await _productProvider.addToCart(productArgs);
                        if (response) {
                          CoolAlert.show(
                            backgroundColor: Colors.blue,
                              context: context,
                              cancelBtnText: "Ir al carrito",
                              showCancelBtn: true,
                              onCancelBtnTap: () {
                                Navigator.pushNamed(context, CartScreen.routeName);
                              },
                              type: CoolAlertType.success,
                              text: productArgs.name +
                                  " ha sido agregado con exito a tu carrito, ve al carrito para ver mas detalles de la compra ",
                              title: "Producto Agregado al Carrito");
                        }
                      },
                    )
                  ],
                ),
              )
            ],
          ),
        ),
      ),
    );
  }
}
