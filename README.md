# 📚 GlassBook API

**GlassBookAPI** is the backend server for **GlassBook**, a social networking platform designed for book lovers. This project was created as part of the **Workshop in Databases** course at **Bar Ilan University**.

The **API server** powers the core functionality of the GlassBook platform, handling user data, book recommendations, reviews, and much more. To run the entire GlassBook application, you will need both this backend and the **client-side code**, which can be found in the [GlassBook Web Client Repository](https://github.com/danielkarl888/glassBook).

---

## 🌟 Overview  

The GlassBook API provides several essential features, including:  
- 📚 Managing user accounts (registration, login).  
- 📖 Storing and retrieving book data.  
- ✍️ Allowing users to leave book reviews and ratings.  
- 🔍 Retrieving popular books based on different criteria like country, category, and more.  

---

## 🛠️ Setup Instructions  

Follow these steps to set up and run the GlassBook API server:  

1. **📂 Clone the repository**:  
   ```bash
   git clone https://github.com/danielkarl888/glassbook_backend.git
   cd glassbook_backend
   ```  

2. **⚙️ Install dependencies**:  
   Ensure that you have **.NET** installed. Run the following command to install all necessary dependencies:  
   ```bash
   dotnet restore
   ```

3. **🗃️ Set up the database**:  
   - Set up a **MySQL** or compatible database.  
   - Import the database schema from the `database` folder.  

4. **🔑 Configure environment variables**:  
   Create a `appsettings.json` file in the root directory and set up your database connection string, along with any other necessary configuration:  
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=glassbook;User Id=root;Password=password;"
     }
   }
   ```

5. **🚀 Run the API server**:  
   Start the server by running the following command:  
   ```bash
   dotnet run
   ```

The API will be available at `http://localhost:5000`.  

---

## 📄 Documentation  

For a comprehensive overview of the GlassBook API, including endpoint details, usage instructions, and setup requirements, refer to the **API documentation** provided in this repository.

---

## 🎥 Video Introduction  

Watch the GlassBook introduction video on YouTube:  

[![Watch on YouTube](https://img.youtube.com/vi/FKjeQNyIu6E/0.jpg)](https://www.youtube.com/watch?v=FKjeQNyIu6E)  

---

## 💻 Technical Details  

- **Framework**: .NET  
- **Database**: MySQL (or other compatible relational databases)  
- **API Structure**: RESTful API  

---
