using Npgsql;
using NpgsqlTypes;
using rabota.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rabota.Services;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<User?> AuthenticateAsync(
    string username,
    string password)
    {
        await using var connection =
        new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        const string sql = @"
           SELECT user_id,
           username,
           password_hash,
           role,
           full_name
           FROM users
           WHERE username = @username
           AND password_hash = @password";

        await using var command =
        new NpgsqlCommand(sql, connection);

        command.Parameters.Add("@username", NpgsqlDbType.Varchar).Value =
        username;

        command.Parameters.Add("@password", NpgsqlDbType.Varchar).Value =
        password;

        await using var reader =
        await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new User
        {
            UserId = reader.GetInt32(0),
            Username = reader.GetString(1),
            PasswordHash = reader.GetString(2),
            Role = reader.GetString(3),
            FullName = reader.GetString(4)
        };
    }

    public async Task<bool> RegisterUserAsync(
    string username,
    string password,
    string role,
    string fullName)
    {
        await using var connection =
        new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        const string sql = @"
           INSERT INTO users
           ( username, password_hash, role, full_name)
           VALUES
           ( @username, @password, @role, @fullName)";

        await using var command =
        new NpgsqlCommand(sql, connection);

        command.Parameters.Add("@username", NpgsqlDbType.Varchar).Value =
        username;

        command.Parameters.Add("@password", NpgsqlDbType.Varchar).Value =
        password;

        command.Parameters.Add("@role", NpgsqlDbType.Varchar).Value =
        role;

        command.Parameters.Add("@fullName", NpgsqlDbType.Varchar).Value =
        fullName;

        await command.ExecuteNonQueryAsync();

        return true;
    }

    public async Task<List<Client>> GetClientsAsync(
    string search = "")
    {
        var result = new List<Client>();

        await using var connection =
        new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        const string sql = @"
           SELECT
           client_id,
           full_name,
           phone,
           email,
           passport_data,
           address,
           birth_date,
           created_at
           FROM clients
           WHERE
           @search = ''
           OR full_name ILIKE '%' || @search || '%'
           OR phone ILIKE '%' || @search || '%'
           OR email ILIKE '%' || @search || '%'
           ORDER BY full_name";

        await using var command =
        new NpgsqlCommand(sql, connection);

        command.Parameters.Add("@search", NpgsqlDbType.Varchar).Value =
        search ?? "";

        await using var reader =
        await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new Client
            {
                ClientId = reader.GetInt32(0),

                FullName = reader.IsDBNull(1)
            ? ""
            : reader.GetString(1),

                Phone = reader.IsDBNull(2)
            ? ""
            : reader.GetString(2),

                Email = reader.IsDBNull(3)
            ? ""
            : reader.GetString(3),

                PassportData = reader.IsDBNull(4)
            ? ""
            : reader.GetString(4),

                Address = reader.IsDBNull(5)
            ? ""
            : reader.GetString(5),

                BirthDate = reader.IsDBNull(6)
            ? null
            : reader.GetDateTime(6),

                CreatedAt = reader.GetDateTime(7)
            });
        }

        return result;
    }

    public async Task AddClientAsync(Client client)
    {
        await using var connection =
        new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        const string sql = @"
           INSERT INTO clients
           ( full_name, phone, email, passport_data, address, birth_date)
           VALUES
           ( @fullName, @phone, @email, @passport, @address, @birthDate)";

        await using var command =
        new NpgsqlCommand(sql, connection);

        command.Parameters.Add("@fullName", NpgsqlDbType.Varchar).Value =
        client.FullName;

        command.Parameters.Add("@phone", NpgsqlDbType.Varchar).Value =
        string.IsNullOrWhiteSpace(client.Phone)
        ? DBNull.Value
        : client.Phone;

        command.Parameters.Add("@email", NpgsqlDbType.Varchar).Value =
        string.IsNullOrWhiteSpace(client.Email)
        ? DBNull.Value
        : client.Email;

        command.Parameters.Add("@passport", NpgsqlDbType.Varchar).Value =
        string.IsNullOrWhiteSpace(client.PassportData)
        ? DBNull.Value
        : client.PassportData;

        command.Parameters.Add("@address", NpgsqlDbType.Varchar).Value =
        string.IsNullOrWhiteSpace(client.Address)
        ? DBNull.Value
        : client.Address;

        command.Parameters.Add("@birthDate", NpgsqlDbType.Date).Value =
        client.BirthDate.HasValue
        ? client.BirthDate.Value.Date
        : DBNull.Value;

        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateClientAsync(Client client)
    {
        await using var connection =
        new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        const string sql = @"
           UPDATE clients
           SET
           full_name = @fullName,
           phone = @phone,
           email = @email,
           passport_data = @passport,
           address = @address,
           birth_date = @birthDate
           WHERE client_id = @clientId";

        await using var command =
        new NpgsqlCommand(sql, connection);

        command.Parameters.Add("@clientId", NpgsqlDbType.Integer).Value =
        client.ClientId;

        command.Parameters.Add("@fullName", NpgsqlDbType.Varchar).Value =
        client.FullName;

        command.Parameters.Add("@phone", NpgsqlDbType.Varchar).Value =
        string.IsNullOrWhiteSpace(client.Phone)
        ? DBNull.Value
        : client.Phone;

        command.Parameters.Add("@email", NpgsqlDbType.Varchar).Value =
        string.IsNullOrWhiteSpace(client.Email)
        ? DBNull.Value
        : client.Email;

        command.Parameters.Add("@passport", NpgsqlDbType.Varchar).Value =
        string.IsNullOrWhiteSpace(client.PassportData)
        ? DBNull.Value
        : client.PassportData;

        command.Parameters.Add("@address", NpgsqlDbType.Varchar).Value =
        string.IsNullOrWhiteSpace(client.Address)
        ? DBNull.Value
        : client.Address;

        command.Parameters.Add("@birthDate", NpgsqlDbType.Date).Value =
        client.BirthDate.HasValue
        ? client.BirthDate.Value.Date
        : DBNull.Value;

        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<Account>> GetAccountsAsync(
    int? clientId = null,
    string? accountType = null,
    string? searchAccountNumber = null,
    bool sortByBalance = false)
    {
        var result = new List<Account>();

        await using var connection =
        new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        string orderBy = sortByBalance
        ? "a.balance DESC"
        : "a.account_id";

        string sql = $@"
           SELECT
           a.account_id,
           a.client_id,
           a.account_number,
           a.account_type,
           a.balance,
           a.status,
           a.opened_at,
           c.full_name
           FROM accounts a
           INNER JOIN clients c
           ON c.client_id = a.client_id
           WHERE
           (@clientId IS NULL OR a.client_id = @clientId)
           AND
           (@accountType IS NULL OR a.account_type = @accountType)
           AND
           ( @search = '' OR a.account_number ILIKE '%' || @search || '%')
           ORDER BY {orderBy}";

        await using var command =
        new NpgsqlCommand(sql, connection);

        command.Parameters.Add(
        new NpgsqlParameter("@clientId", NpgsqlDbType.Integer)
        {
            Value = clientId.HasValue
        ? clientId.Value
        : DBNull.Value
        });

        command.Parameters.Add(
        new NpgsqlParameter("@accountType", NpgsqlDbType.Varchar)
        {
            Value = string.IsNullOrWhiteSpace(accountType)
        ? DBNull.Value
        : accountType
        });

        command.Parameters.Add(
        new NpgsqlParameter("@search", NpgsqlDbType.Varchar)
        {
            Value = searchAccountNumber ?? ""
        });

        await using var reader =
        await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new Account
            {
                AccountId = reader.GetInt32(0),
                ClientId = reader.GetInt32(1),
                AccountNumber = reader.GetString(2),
                AccountType = reader.GetString(3),
                Balance = reader.GetDecimal(4),
                Status = reader.GetString(5),
                OpenedAt = reader.GetDateTime(6),
                ClientName = reader.GetString(7)
            });
        }

        return result;
    }

    public async Task<List<string>> GetAccountTypesAsync()
    {
        var result = new List<string>();

        await using var connection =
        new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        const string sql = @"
           SELECT DISTINCT account_type
           FROM accounts
           ORDER BY account_type";

        await using var command =
        new NpgsqlCommand(sql, connection);

        await using var reader =
        await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(reader.GetString(0));
        }

        return result;
    }

    public async Task<List<TransactionItem>> GetTransactionsAsync(
    int? accountId = null)
    {
        var result = new List<TransactionItem>();

        await using var connection =
        new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        const string sql = @"
           SELECT
           t.transaction_id,
           t.account_id,
           a.account_number,
           t.transaction_type,
           t.amount,
           t.description,
           t.created_at
           FROM transactions t
           INNER JOIN accounts a
           ON a.account_id = t.account_id
           WHERE
           @accountId IS NULL
           OR t.account_id = @accountId
           ORDER BY t.created_at DESC";

        await using var command =
        new NpgsqlCommand(sql, connection);

        command.Parameters.Add(
        new NpgsqlParameter("@accountId", NpgsqlDbType.Integer)
        {
            Value = accountId.HasValue
        ? accountId.Value
        : DBNull.Value
        });

        await using var reader =
        await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new TransactionItem
            {
                TransactionId = reader.GetInt32(0),
                AccountId = reader.GetInt32(1),
                AccountNumber = reader.GetString(2),
                TransactionType = reader.GetString(3),
                Amount = reader.GetDecimal(4),

                Description = reader.IsDBNull(5)
            ? ""
            : reader.GetString(5),

                CreatedAt = reader.GetDateTime(6)
            });
        }

        return result;
    }

    public async Task CreateTransactionAsync(
    string senderAccountNumber,
    string receiverAccountNumber,
    decimal amount,
    string description)
    {
        if (amount <= 0)
            throw new Exception(
            "Сумма перевода должна быть больше нуля.");

        if (string.IsNullOrWhiteSpace(senderAccountNumber))
            throw new Exception(
            "Введите счет отправителя.");

        if (string.IsNullOrWhiteSpace(receiverAccountNumber))
            throw new Exception(
            "Введите счет получателя.");

        if (senderAccountNumber == receiverAccountNumber)
            throw new Exception(
            "Нельзя переводить деньги на тот же счет.");

        await using var connection =
        new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        await using var transaction =
        await connection.BeginTransactionAsync();

        try
        {
            const string findAccountSql = @"
               SELECT
               account_id,
               balance,
               status
               FROM accounts
               WHERE account_number = @number
               FOR UPDATE";

            int senderId;
            decimal senderBalance;
            string senderStatus;

            await using (
            var command = new NpgsqlCommand(
            findAccountSql,
            connection,
            transaction))
            {
                command.Parameters.Add(
                "@number",
                NpgsqlDbType.Varchar).Value =
                senderAccountNumber;

                await using var reader =
                await command.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                {
                    throw new Exception(
                    "Счет отправителя не найден.");
                }

                senderId = reader.GetInt32(0);
                senderBalance = reader.GetDecimal(1);
                senderStatus = reader.GetString(2);
            }

            if (senderStatus != "Активен")
                throw new Exception(
                "Счет отправителя не активен.");

            int receiverId;
            string receiverStatus;

            await using (
            var command = new NpgsqlCommand(
            findAccountSql,
            connection,
            transaction))
            {
                command.Parameters.Add(
                "@number",
                NpgsqlDbType.Varchar).Value =
                receiverAccountNumber;

                await using var reader =
                await command.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                {
                    throw new Exception(
                    "Счет получателя не найден.");
                }

                receiverId = reader.GetInt32(0);
                receiverStatus = reader.GetString(2);
            }

            if (receiverStatus != "Активен")
                throw new Exception(
                "Счет получателя не активен.");

            if (senderId == receiverId)
                throw new Exception(
                "Нельзя переводить деньги на тот же счет.");

            if (senderBalance < amount)
                throw new Exception(
                "Недостаточно средств на счете отправителя.");

            const string updateSenderSql = @"
               UPDATE accounts
               SET balance = balance - @amount
               WHERE account_id = @accountId";

            await using (
            var command = new NpgsqlCommand(
            updateSenderSql,
            connection,
            transaction))
            {
                command.Parameters.Add(
                "@amount",
                NpgsqlDbType.Numeric).Value =
                amount;

                command.Parameters.Add(
                "@accountId",
                NpgsqlDbType.Integer).Value =
                senderId;

                await command.ExecuteNonQueryAsync();
            }

            const string updateReceiverSql = @"
               UPDATE accounts
               SET balance = balance + @amount
               WHERE account_id = @accountId";

            await using (
            var command = new NpgsqlCommand(
            updateReceiverSql,
            connection,
            transaction))
            {
                command.Parameters.Add(
                "@amount",
                NpgsqlDbType.Numeric).Value =
                amount;

                command.Parameters.Add(
                "@accountId",
                NpgsqlDbType.Integer).Value =
                receiverId;

                await command.ExecuteNonQueryAsync();
            }

            const string insertTransactionSql = @"
               INSERT INTO transactions
               ( account_id, transaction_type, amount, description)
               VALUES
               ( @accountId, @type, @amount, @description )";

            await using (
            var command = new NpgsqlCommand(
            insertTransactionSql,
            connection,
            transaction))
            {
                command.Parameters.Add(
                "@accountId",
                NpgsqlDbType.Integer).Value =
                senderId;

                command.Parameters.Add(
                "@type",
                NpgsqlDbType.Varchar).Value =
                "Перевод";

                command.Parameters.Add(
                "@amount",
                NpgsqlDbType.Numeric).Value =
                amount;

                command.Parameters.Add(
                "@description",
                NpgsqlDbType.Varchar).Value =
                description ?? "";

                await command.ExecuteNonQueryAsync();
            }

            await using (
            var command = new NpgsqlCommand(
            insertTransactionSql,
            connection,
            transaction))
            {
                command.Parameters.Add(
                "@accountId",
                NpgsqlDbType.Integer).Value =
                receiverId;

                command.Parameters.Add(
                "@type",
                NpgsqlDbType.Varchar).Value =
                "Зачисление";

                command.Parameters.Add(
                "@amount",
                NpgsqlDbType.Numeric).Value =
                amount;

                command.Parameters.Add(
                "@description",
                NpgsqlDbType.Varchar).Value =
                description ?? "";

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<Loan>> GetLoansAsync()
    {
        var result = new List<Loan>();

        await using var connection =
        new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        const string sql = @"
           SELECT
           l.loan_id,
           l.client_id,
           l.loan_number,
           l.amount,
           l.interest_rate,
           l.remaining_amount,
           l.status,
           l.start_date,
           l.end_date,
           c.full_name
           FROM loans l
           INNER JOIN clients c
           ON c.client_id = l.client_id
           ORDER BY l.start_date DESC";

        await using var command =
        new NpgsqlCommand(sql, connection);

        await using var reader =
        await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new Loan
            {
                LoanId = reader.GetInt32(0),
                ClientId = reader.GetInt32(1),
                LoanNumber = reader.GetString(2),
                Amount = reader.GetDecimal(3),
                InterestRate = reader.GetDecimal(4),
                RemainingAmount = reader.GetDecimal(5),
                Status = reader.GetString(6),
                StartDate = reader.GetDateTime(7),

                EndDate = reader.IsDBNull(8)
            ? null
            : reader.GetDateTime(8),

                ClientName = reader.GetString(9)
            });
        }

        return result;
    }

    public async Task<List<Deposit>> GetDepositsAsync()
    {
        var result = new List<Deposit>();

        await using var connection =
        new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        const string sql = @"
           SELECT
           d.deposit_id,
           d.client_id,
           d.deposit_number,
           d.amount,
           d.interest_rate,
           d.status,
           d.start_date,
           d.end_date,
           c.full_name
           FROM deposits d
           INNER JOIN clients c
           ON c.client_id = d.client_id
           ORDER BY d.start_date DESC";

        await using var command =
        new NpgsqlCommand(sql, connection);

        await using var reader =
        await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new Deposit
            {
                DepositId = reader.GetInt32(0),
                ClientId = reader.GetInt32(1),
                DepositNumber = reader.GetString(2),
                Amount = reader.GetDecimal(3),
                InterestRate = reader.GetDecimal(4),
                Status = reader.GetString(5),
                StartDate = reader.GetDateTime(6),

                EndDate = reader.IsDBNull(7)
            ? null
            : reader.GetDateTime(7),

                ClientName = reader.GetString(8)
            });
        }

        return result;
    }

    public async Task<List<Employee>> GetEmployeesAsync()
    {
        var result = new List<Employee>();

        await using var connection =
        new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        const string sql = @"
           SELECT
           e.employee_id,
           e.full_name,
           e.position,
           e.phone,
           e.email,
           e.branch_id,
           e.hire_date,
           COALESCE(b.name, '')
           FROM employees e
           LEFT JOIN branches b
           ON b.branch_id = e.branch_id
           ORDER BY e.full_name";

        await using var command =
        new NpgsqlCommand(sql, connection);

        await using var reader =
        await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new Employee
            {
                EmployeeId = reader.GetInt32(0),
                FullName = reader.GetString(1),
                Position = reader.GetString(2),

                Phone = reader.IsDBNull(3)
            ? ""
            : reader.GetString(3),

                Email = reader.IsDBNull(4)
            ? ""
            : reader.GetString(4),

                BranchId = reader.IsDBNull(5)
            ? null
            : reader.GetInt32(5),

                HireDate = reader.GetDateTime(6),
                BranchName = reader.GetString(7)
            });
        }

        return result;
    }

    public async Task<List<Branch>> GetBranchesAsync()
    {
        var result = new List<Branch>();

        await using var connection =
        new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        const string sql = @"
           SELECT
           branch_id,
           name,
           address,
           phone,
           manager_name
           FROM branches
           ORDER BY name";

        await using var command =
        new NpgsqlCommand(sql, connection);

        await using var reader =
        await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new Branch
            {
                BranchId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Address = reader.GetString(2),

                Phone = reader.IsDBNull(3)
            ? ""
            : reader.GetString(3),

                ManagerName = reader.IsDBNull(4)
            ? ""
            : reader.GetString(4)
            });
        }

        return result;
    }

    public async Task<List<Payment>> GetPaymentsAsync(
    string? accountNumber = null)
    {
        var result = new List<Payment>();

        await using var connection =
        new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        const string sql = @"
           SELECT
           p.payment_id,
           p.account_id,
           p.amount,
           p.purpose,
           p.status,
           p.payment_date,
           a.account_number
           FROM payments p
           INNER JOIN accounts a
           ON a.account_id = p.account_id
           WHERE
           @accountNumber IS NULL
           OR a.account_number = @accountNumber
           ORDER BY p.payment_date DESC";

        await using var command =
        new NpgsqlCommand(sql, connection);

        command.Parameters.Add(
        new NpgsqlParameter(
        "@accountNumber",
        NpgsqlDbType.Varchar)
        {
            Value = string.IsNullOrWhiteSpace(accountNumber)
        ? DBNull.Value
        : accountNumber
        });

        await using var reader =
        await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new Payment
            {
                PaymentId = reader.GetInt32(0),
                AccountId = reader.GetInt32(1),
                Amount = reader.GetDecimal(2),

                Purpose = reader.IsDBNull(3)
            ? ""
            : reader.GetString(3),

                Status = reader.GetString(4),
                PaymentDate = reader.GetDateTime(5),
                AccountNumber = reader.GetString(6)
            });
        }

        return result;
    }
}