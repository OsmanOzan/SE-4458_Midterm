# SE-4458_Midterm
Design, Assumptions, and Issues
Design Decisions
The application is designed as a RESTful API with layered architecture (Controllers, DTOs, Models, Data).

Swagger UI is used for testing and documentation.

Authentication is handled using JWT bearer tokens.

API versioning is enabled (v1.0), making the project easily extendable.

MySQL database is used for all data persistence.

Assumptions
Each subscriber has monthly phone and internet usage.

Phone usage up to 1000 minutes is free; every extra 1000 minutes costs $10.

Internet usage up to 20GB (20000 MB) is $50; every 10GB (10000 MB) above costs $10.

Bills are calculated per month, per subscriber.

Issues Encountered
Faced early problems with Azure Database deployment, since Azure MySQL integration requires a paid plan.

Swagger initially did not show Authorize button – fixed by configuring AddSecurityDefinition() properly.

JWT HS256 key size error resolved by extending secret key to 32 characters (256 bits).

After the first attempt, authorization didn’t work in the following attempts.

I couldn’t upload the entire project to GitHub due to file size limitations.

Data model:
Subscribers (subscriber_no PK)
│
├── Usage (id PK, subscriber_no FK)
│
├── Bills (id PK, subscriber_no FK)
│
└── BillPayments (id PK, bill_id FK)


Video link: https://youtu.be/Q-Rcut0MxHA

Swagger link: https://midterm20250423001735-esc6hca4a2hnakfy.francecentral-01.azurewebsites.net/swagger/index.html

Authentication details:
{
  "username": "osman",
  "password": "12345"
}

MySQL part:
create SCHEMA midterm;
use midterm;
-- 1. Subscribers
CREATE TABLE Subscribers (
    subscriber_no INT PRIMARY KEY,
    name VARCHAR(100)
);

-- 2. Usage
CREATE TABLE `Usage` (
    id INT AUTO_INCREMENT PRIMARY KEY,
    subscriber_no INT,
    year INT,
    month INT,
    usage_type ENUM('Phone', 'Internet'),
    amount INT, -- phone için dakika, internet için MB
    FOREIGN KEY (subscriber_no) REFERENCES Subscribers(subscriber_no)
);

-- 3. Bills
CREATE TABLE Bills (
    id INT AUTO_INCREMENT PRIMARY KEY,
    subscriber_no INT,
    year INT,
    month INT,
    phone_minutes_used INT DEFAULT 0,
    internet_used_mb INT DEFAULT 0,
    total_amount DECIMAL(10,2),
    is_paid BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (subscriber_no) REFERENCES Subscribers(subscriber_no)
);

-- 4. BillPayments
CREATE TABLE BillPayments (
    id INT AUTO_INCREMENT PRIMARY KEY,
    bill_id INT,
    payment_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    amount_paid DECIMAL(10,2),
    FOREIGN KEY (bill_id) REFERENCES Bills(id)
);

INSERT INTO Subscribers (subscriber_no, name) VALUES
(1001, 'Ali Yılmaz'),
(1002, 'Ayşe Demir'),
(1003, 'Mehmet Kaya'),
(1004, 'Elif Çetin'),
(1005, 'Ahmet Şahin');

INSERT INTO `Usage` (subscriber_no, year, month, usage_type, amount) VALUES
(1001, 2025, 4, 'Phone', 800),
(1001, 2025, 4, 'Internet', 18000),
(1002, 2025, 4, 'Phone', 1200),
(1002, 2025, 4, 'Internet', 25000),
(1003, 2025, 4, 'Phone', 500),
(1003, 2025, 4, 'Internet', 15000),
(1004, 2025, 4, 'Phone', 2000),
(1004, 2025, 4, 'Internet', 40000),
(1005, 2025, 4, 'Phone', 1000),
(1005, 2025, 4, 'Internet', 20000);

INSERT INTO Bills (subscriber_no, year, month, phone_minutes_used, internet_used_mb, total_amount, is_paid) VALUES
(1001, 2025, 4, 800, 18000, 50.00, TRUE),         -- Phone free, 18GB = $50
(1002, 2025, 4, 1200, 25000, 65.00, FALSE),       -- 200 mins = $10, 25GB = $50 + $10
(1003, 2025, 4, 500, 15000, 50.00, TRUE),         -- Phone free, 15GB = $50
(1004, 2025, 4, 2000, 40000, 90.00, FALSE),       -- 1000 mins = $10, 40GB = $50 + $30
(1005, 2025, 4, 1000, 20000, 50.00, FALSE);       -- Phone free, 20GB = $50

INSERT INTO BillPayments (bill_id, amount_paid) VALUES
(1, 50.00),
(3, 50.00);

SELECT * FROM `Usage`;
