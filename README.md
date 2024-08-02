# Azure Functions

Este repositorio contiene tres funciones de Azure: FunctionApp1, FunctionApp2 y FunctionApp3.  
El funcionamiento base es similar en todas ellas: se activa mediante un disparador HTTP GET o POST, lee el parámetro `nombre` de la cadena de consulta o del cuerpo de la solicitud, genera un mensaje y devuelve el mensaje en la respuesta HTTP.  

  FunctionApp2, añade una característica adicional, guardando un registro en una tabla de almacenamiento de Azure.  
  FunctionApp3 es similar a FunctionApp2 pero utilizando el atributo `TableOutput` para simplificar la salida de datos.
