# Eshop-console-app
C# console app for a simple e-shop

## Description
C# console app for a simple e-shop. There are two entities in the e-shop - products and categories. Each entity has its ID and name, and the product also has a price. In addition, the application writes analytical data about these entities and logs from each executed command to the files. To try functionality use following comamnds:

```bash
add-product <name> <categoryID> <price>
# creates a new product with the given parameters

delete-product <productID>
# deletes the product with the given ID

list-products
# displays a list of all current products

add-category <name>
# creates a new category with the given name (the ID is assigned to the category itself)

delete-category <categoryID>
# deletes the category with the given ID

list-categories
# displays a list of all current categories

get-products-by-category <categoryID>
# displays a list of all products in the category with the given ID
```
