-- One to May | Account has many recipes
CREATE TABLE IF NOT EXISTS accounts (
    id VARCHAR(255) NOT NULL PRIMARY KEY,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    name VARCHAR(255),
    email VARCHAR(255) UNIQUE,
    picture VARCHAR(255)
) default charset utf8mb4;

SELECT * FROM accounts

DROP TABLE recipes;

CREATE TABLE recipes (
    id INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    title VARCHAR(255) NOT NULL,
    instructions VARCHAR(5000) NOT NULL,
    img VARCHAR(1000) NOT NULL,
    category ENUM(
        'breakfast',
        'lunch',
        'dinner',
        'snack',
        'dessert'
    ) NOT NULL,
    creatorId VARCHAR(255) NOT NULL,
    FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE
)

SELECT * FROM recipes

DROP TABLE ingredients
-- One to Many | Recipe has many ingredients
CREATE TABLE ingredients (
    id INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    name VARCHAR(255) NOT NULL,
    quantity VARCHAR(255) NOT NULL,
    recipeId INT UNSIGNED NOT NULL,
    FOREIGN KEY (recipeId) REFERENCES recipes (id) ON DELETE CASCADE
)
-- Many to Many | Favorite joins Recipe & Account
DROP TABLE favorites;

CREATE TABLE favorites (
    id INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    recipeId INT UNSIGNED NOT NULL,
    accountId VARCHAR(255) NOT NULL,
    FOREIGN KEY (recipeId) REFERENCES recipes (id) ON DELETE CASCADE,
    FOREIGN KEY (accountId) REFERENCES accounts (id) ON DELETE CASCADE UNIQUE (recipeId, accountId)
)

SELECT favorites.*, recipes.*, accounts.*
FROM
    favorites
    INNER JOIN recipes ON favorites.recipeId = recipes.id
    INNER JOIN accounts ON favorites.accountId = accounts.id
WHERE
    favorites.id = 1;

SELECT * FROM recipes;

SELECT * FROM ingredients;

ALTER TABLE favorites ADD CONSTRAINT UNIQUE (recipeId, accountId)