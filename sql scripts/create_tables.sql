CREATE  table if not EXISTS users (
                                      id char not null primary key UNIQUE,
                                      name char not null,
                                      family_name char not null,
                                      pay_account_id char not null UNIQUE
                                     );

CREATE  table if not EXISTS flights (
                                      id char not null primary key UNIQUE,
                                      city char not null,
                                      week_day NUMERIC not null,
                                      price NUMERIC not null
                                     );

CREATE  table if not EXISTS accounts (
                                      id char not null primary key UNIQUE,
                                      amount NUMERIC not null,
                                      user_id char not null,
									  FOREIGN KEY (user_id)  REFERENCES users (id)
                                     );