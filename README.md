# Getting Started

Follow the steps below to set up and run the application using Docker.

---

## Step 1: Create `.env` file in the root directory

Create a `.env` file in the project root by copying the example:

```bash
cp .env.example .env
```

Update values if needed (e.g., ports).

---

## Step 2: Set up frontend environment variables

In the `frontend/` directory, create a `.env` file from `.env.example`:

```bash
cp frontend/.env.example frontend/.env
```

Set the backend URL using the `BACKEND_PORT_1` value from the root `.env` file.

**Example:**

```env
VITE_API_URL=http://localhost:5050
```

---

## Step 3: Configure Telegram credentials

Edit the file `backend/Veloci.Web/appsettings.json` and provide the required values:

```json
"Telegram": {
  "BotToken": "",
  "ChannelId": ""
}
```

---

## Step 4: Build and start the application

From the project root directory, run:

```bash
docker-compose build && docker-compose up -d
```

---

## Step 5: Run database migrations

Open [http://localhost:5050/](http://localhost:5050/) in your browser and run the migrations.

---

## Step 6: Open the frontend

Visit the frontend in your browser:

[http://localhost:3001/](http://localhost:3001/)

---