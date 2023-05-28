# MyShopProject_WindowsDevelopment
Đồ án 1: MyShop môn Lập trình Windows

# DB:
![image](https://user-images.githubusercontent.com/103125520/229202771-8614ab67-fbd0-458c-96f2-ce43a7156d37.png)

##### Lecturer Instruction: **Mr Trần Duy Quang**
###### _Web I used to write this markdown file: https://dillinger.io_ (You should use this website to ensure the correct formatting for display. )
# ==_PROJECT 1 - BOOK SHOP MANAGEMENT_==
#
#
#### ++1. Members of the group++
1. 20127533 - Lê Đăng Khoa
2. 20127475 - Nguyễn Trần Đại Dương
3. 20127596 - Nguyễn Như Phước
4. 20127599 - Lê Quân
****
#### ++2. Important points to remember when running the program++
+  **++_My Shop Project_++** uses many UI components from the [`Telerik`](https://www.telerik.com/products/wpf/overview.aspx) library. Because this library is a paid service, the group downloaded it illegally from a third-party source for educational purposes. Therefore, if you want to compile the program directly from the code, you can download the library via the following link: [`Link download from my Google drive`](https://drive.google.com/drive/folders/1FeuZlBZSiXBlq1l5fDT_VZplDZ0NMvPP?usp=sharing) 
+ Project uses NodeJs, Express and MongoDB to create a REST API connect with Application
+ There two ways to run REST API server:
    - First way: Use Visual Studio Code to run server **++_(This way can only be used when your computer has installed Nodejs)_++**
        --Step 1: Open the **API_MyShopProject_WindowsDevelopment** folder in **API** folder with **Visual Studio Code** and Create a new terminal.
        --Step 2: Enter **"npm i"** to install all the packages
        --Step 3 Enter **"npm run dev"** to run the server
        --This is the result after Step 3:
            ![Imgur](https://i.imgur.com/0FkoHGv.png)
    - Second ways: Use docker to run server
        -- Step 1: Open Docker Desktop app
        -- Step 2:  Open Command Prompt and enter 
            "docker image pull ngtrdaiduong2162002/my_shop_database" for pulling image on docker hub to your computer
             ![Imgur](https://i.imgur.com/bMfPUr6.png)
        -- Step 3: Then enter "docker run -p 5000:5000 ngtrdaiduong2162002/my_shop_database" to Command Prompt run the server
            ![Imgur](https://i.imgur.com/kQYBh4k.png)
+ There are 2 ways to run application
-- Run executable file `a_MyShopProject.exe` from `Release Folder` (prefix `a_` in filename to make it appear at top, easier for teachers to see)
 [![N|Solid](https://i.imgur.com/ileZQOJ.png)](https://i.imgur.com/ileZQOJ.png)
-- Install `Setup` file from `Setup folder`. When install app, a shortcut to run app will appear at your Desktop
==+ If you run the app using method 2, make sure the app Run as administrator (Group have already set requestedExecutionLevel in manifest is `requireAdministrator`)==

+ You can register new account or using available account:
-- Account - Password 1: `khoa` - `123789`
-- Account - Password 2: `phuoc` - `123456`
****
#### ++3. Features that the group has completed++
##### _Basic features_
1. **Login - Sign up**: Having feature to remember the username and password for future logins. The password stored in the database has been encrypted.
2. **Dashboard Screen**: There are multiple statistics on the dashboard screen such as (total books, orders, revenue, profit,...), and it uses various visual interfaces such as charts with legend and carousels.
[![N|Solid](https://i.imgur.com/w7LTapo.png)](https://i.imgur.com/w7LTapo.png)

3. **Books Management**: Using CartView component to display each book (Each CartView can collapse, expand, scroll itself)
 [![N|Solid](https://i.imgur.com/ZAGWxa6.png)](https://i.imgur.com/ZAGWxa6.png)
-- Because Book's cover size is large. So application will show `text` information first and loading image afterwards. Provide a fast response and improve the user's product loading experience. Indicator progress will show during loading time.
-- The product list is clearly paginated and uses DataPager component (Telerik) for page navigation.
-- Search bar helps customer search Book by name.
-- Advanced filter helps filtering product by Range price, Categories selected.
  [![N|Solid](https://i.imgur.com/FFP4UQ1.png)](https://i.imgur.com/FFP4UQ1.png)
-- In this screen, user can add new Book, right mouse click to open context menu with 2 options Edit or Delete this book. (There is a confirmation dialog before deleting a product)
  [![N|Solid](https://i.imgur.com/PlndlKu.png)](https://i.imgur.com/PlndlKu.png)

4. **Categories Management**: RadGridView (to view categories in a table format) and RadDataForm (detailed category form supporting add, delete, and edit functions) are combined in parallel. The item selected on RadGridView is then bound to the form below.
 [![N|Solid](https://i.imgur.com/ipI5STC.png)](https://i.imgur.com/ipI5STC.png)

5. **Orders Management**: Add new order screen uses Telerik library highly efficient, with the MultiColumnCombobox allowing users to select multiple books and adjust quantities, with automatic calculation and display of the total amount
[![N|Solid](https://i.imgur.com/rAZ5eUK.png)](https://i.imgur.com/rAZ5eUK.png)
-- User can add coupon discount for this order by select available coupon or create new coupon
[![N|Solid](https://i.imgur.com/1XHzB9Z.png)](https://i.imgur.com/1XHzB9Z.png)
-- The order list will be displayed as a paginated table. User can Edit or Delete order in `Action  column`
[![N|Solid](https://i.imgur.com/5LaBvoz.png)](https://i.imgur.com/5LaBvoz.png)
-- The application provides users with the function to search for orders within a time range by clicking on the button in the top right corner.
-- User can double click each order to view detail
[![N|Solid](https://i.imgur.com/so3wb1E.png)](https://i.imgur.com/so3wb1E.png)

6. **Statistics**: There are two types of statistics on this screen: sales quantity statistics per product and profit statistics (selling price - purchase price).
-- With `sales quantity statistics per product`, we use a multi-line chart to allow users to compare multiple products with each other, using a legend to color-code each product. Users can choose to display statistics within a time range, by month-year or for the entire year.
[![N|Solid](https://i.imgur.com/XZiF1sc.png)](https://i.imgur.com/XZiF1sc.png)
-- For profit statistics, we use a bar chart and also allow users to select the time range for the statistics.
[![N|Solid](https://i.imgur.com/am0QRU2.png)](https://i.imgur.com/am0QRU2.png)

7. **Import data from an Excel file**: In reality, data is stored in a file with multiple sheets. Therefore, the app allows users to select which sheet to add data from. 
[![N|Solid](https://i.imgur.com/HIcGcDd.png)](https://i.imgur.com/HIcGcDd.png)

7. **Setting**: Users can choose the number of books per page and the number of orders per page through the settings button (gear icon) located at the top right corner

8. *Remember last tab*: When app runs, the final tab user used will appear first
8. *Packaging into an installation file*: My group has packaged the application into an installation file like real desktop applications
##### _Advanced Features:_
1. **Report the best-selling products of the week, the month, and the year**: In dashboard, user can see best selling products with Carousel view
[![N|Solid](https://i.imgur.com/I9lHPWG.png)](https://i.imgur.com/I9lHPWG.png)

2. **Discount**: When create new order or edit order, users can apply a discount code to receive a percentage discount on their total order.
 [![N|Solid](https://i.imgur.com/TZacbaS.png)](https://i.imgur.com/TZacbaS.png)

3. **Using 2-layers + 3-tier architecture model**: The application is implemented as 3 separate modules: GUI (Graphical User Interface), DTO (Data Transfer Object), BUS (Business logic), and DAO (Data Access Object) connect with REST API webservice.
 [![N|Solid](https://i.imgur.com/Zysn8Tg.png)](https://i.imgur.com/Zysn8Tg.png)

4. **Using Telerik UI Library**: The Telerik library has over 140 interface components, making it easier to use the application. The most prominent ones include MultiColumnCombobox, RadGridView, RadForm, RadPager, ...
 [![N|Solid](https://i.imgur.com/jIlVorK.png)](https://i.imgur.com/jIlVorK.png)

5. **Connecting to a Rest API**: In the application, the DAO layer will make HTTP requests to APIs built by the team through Node and ExpressJS to interact with MongoDb.
 [![N|Solid](https://i.imgur.com/aK6xobI.png)](https://i.imgur.com/aK6xobI.png)

#### ✨++Expectecd Grade: 
1. 20127533 - Lê Đăng Khoa : 10đ
2. 20127475 - Nguyễn Trần Đại Dương: 10đ
3. 20127596 - Nguyễn Như Phước: 10đ
4. 20127599 - Lê Quân: 10đ
#
#### ✨++Video demo Link (`Youtube`):++ https://youtu.be/EgYqZwT5nkM
+ **_Please enable the subtitles for a detailed explanation_**
### `Thank you for reading our report `
## _END_
