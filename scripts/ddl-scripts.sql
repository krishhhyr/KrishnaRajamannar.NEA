CREATE TABLE UserDetails (
    userID         INTEGER PRIMARY KEY
                           UNIQUE
                           NOT NULL,
    username       TEXT    UNIQUE
                           NOT NULL,
    password       TEXT    NOT NULL,
    numberOfPoints INTEGER NOT NULL
);
