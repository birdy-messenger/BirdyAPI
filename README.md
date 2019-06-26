# BirdyAPI

Аутентификация :
INPUT
https://birdytestapi.azurewebsites.net/api/auth?email=XXXX&passwordHash=YYYY
Где XXXX - почта, на которую регистрировались, YYYY - хэш от пароля
OUTPUT
Если данные верны, придет Ok(status code 200) с JSON, в котором лежит токен и Id 
+ есть ErrorMessage = null 
Если данные НЕ верны, придет BadRequest(status code 400) с JSON, в котором ErrorMessage = "Invalid email or password" , 
остальные поля в дефолтных значениях

Регистрация : 
INPUT
https://birdytestapi.azurewebsites.net/api/user.reg?email=XXXX&passwordHash=YYYY&firstname=ZZZZ
Где XXXX - почта, на которую регистрируются, YYYY - хэш от пароля, ZZZZ - имя для регистрации
OUTPUT
Если регистрация прошла успешна, вернется Ok(status code 200) с JSON, в котором лежит FirstName = указанному при регистрации
+ ErrorMessage = null
Если регистрация НЕ прошла, вернется BadRequest(status code 400) с JSON, в котором ErrorMessage = "Duplicate account", FirstName = null
