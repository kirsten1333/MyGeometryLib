# Задание 2
Как я понимаю, тут нужно сделать допущение, раз связь продуктов и категорий многие ко многим, то необходимо добавить таблицу, отображающую связи этих таблиц. Тогда получится такой запрос.

```SQL
SELECT Product.name AS Product_Name, Category.name AS Category_Name
FROM Product
LEFT JOIN ProductCategory AS PC
ON Product.id = PC.Product_id 
LEFT JOIN Category
ON PC.Category_id = Category.id
```
Если нам необходимо вывести и категории без продуктов, то второй LEFT JOIN следует заменить на FULL JOIN.

DML для создания таблиц:
```SQL
CREATE TABLE Category(
id INT NOT NULL,
name VARCHAR(100) NOT NULL
PRIMARY KEY(id)
)

CREATE TABLE Product(
id INT NOT NULL,
name VARCHAR(100) NOT NULL
PRIMARY KEY(id)
)

CREATE TABLE ProductCategory(
id INT NOT NULL,
Product_id int NOT NULL,
Category_id int NOT Null,
PRIMARY KEY(id),
FOREIGN Key(Product_id) REFERENCES Product(id),
FOREIGN Key(Category_id) REFERENCES Category(id)
)
```

Тестовое наполнение таблиц:
```SQL
INSERT INTO ProductCategory (id,Product_id,Category_id) VALUES ('1','1','1'); 
INSERT INTO ProductCategory (id,Product_id,Category_id) VALUES ('2','1','2'); 
INSERT INTO ProductCategory (id,Product_id,Category_id) VALUES ('3','1','3'); 
INSERT INTO ProductCategory (id,Product_id,Category_id) VALUES ('4','1','4'); 
INSERT INTO ProductCategory (id,Product_id,Category_id) VALUES ('5','2','5'); 
INSERT INTO ProductCategory (id,Product_id,Category_id) VALUES ('6','2','6'); 
INSERT INTO ProductCategory (id,Product_id,Category_id) VALUES ('7','2','4'); 
INSERT INTO ProductCategory (id,Product_id,Category_id) VALUES ('8','3','3'); 
INSERT INTO ProductCategory (id,Product_id,Category_id) VALUES ('9','4','4'); 
INSERT INTO ProductCategory (id,Product_id,Category_id) VALUES ('10','5','6'); 
INSERT INTO ProductCategory (id,Product_id,Category_id) VALUES ('11','5','2'); 
INSERT INTO ProductCategory (id,Product_id,Category_id) VALUES ('12','5','3'); 
SELECT * FROM ProductCategory;

INSERT INTO Product (id,name) VALUES ('1', 'Product1');
INSERT INTO Product (id,name) VALUES ('2', 'Product2');
INSERT INTO Product (id,name) VALUES ('3', 'Product3');
INSERT INTO Product (id,name) VALUES ('4', 'Product4');
INSERT INTO Product (id,name) VALUES ('5', 'Product5');
INSERT INTO Product (id,name) VALUES ('6', 'Product6');
INSERT INTO Product (id,name) VALUES ('7', 'Product7');
SELECT * FROM Product;

INSERT INTO Category (id,name) VALUES ('1', 'Category1');
INSERT INTO Category (id,name) VALUES ('2', 'Category2');
INSERT INTO Category (id,name) VALUES ('3', 'Category3');
INSERT INTO Category (id,name) VALUES ('4', 'Category4');
INSERT INTO Category (id,name) VALUES ('5', 'Category5');
INSERT INTO Category (id,name) VALUES ('6', 'Category6');
INSERT INTO Category (id,name) VALUES ('7', 'Category7');
SELECT * FROM Category;
```
