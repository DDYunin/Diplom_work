#include <Arduino.h>
#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>
#include <cmath>
#include <package.h>

myPackage pack;
PackageController controller;

// даем название BLE-серверу:
#define bleServerName "DAN_MY_ESP32"


// для генерирования UUID можно воспользоваться этим сайтом:
// https://www.uuidgenerator.net/
#define SERVICE_UUID "17b5290c-2e8b-11ed-a261-0242ac120002"

#define CHARACTERISTIC_X "17b52c72-2e8b-11eb-a261-0242ac120002" 
#define CHARACTERISTIC_Y "17b52c72-2e8b-11ef-a261-0242ac120002" 

BLECharacteristic xPointCharacteristics(CHARACTERISTIC_X, BLECharacteristic::PROPERTY_INDICATE);
BLEDescriptor xPointDescriptor(BLEUUID((uint16_t)0x2903));

BLECharacteristic yPointCharacteristics(CHARACTERISTIC_Y, BLECharacteristic::PROPERTY_INDICATE);
BLEDescriptor yPointDescriptor(BLEUUID((uint16_t)0x2904));

bool deviceConnected = false;

// задаем функции обратного вызова onConnect() и onDisconnect():
class MyServerCallbacks: public BLEServerCallbacks {
  void onConnect(BLEServer* pServer) {
    deviceConnected = true;
  };
  void onDisconnect(BLEServer* pServer) {
    deviceConnected = false;
  }
};

BLEServer *pServer;

void setup() {
  // запускаем последовательную коммуникацию:
  Serial.begin(115200);

  // создаем BLE-устройство:
  BLEDevice::init(bleServerName);

  // создаем BLE-сервер:
  pServer = BLEDevice::createServer();
  pServer->setCallbacks(new MyServerCallbacks());

  // создаем BLE-сервис:
  BLEService *pointsService = pServer->createService(SERVICE_UUID);
  // создаем BLE-характеристики и BLE-дескриптор: bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.descriptor.gatt.client_characteristic_configuration.xml 
  pointsService->addCharacteristic(&xPointCharacteristics);
  xPointDescriptor.setValue("Sin(x) functions - points x"); 
  xPointCharacteristics.addDescriptor(new BLE2902()); //без него не будет работать notify
  xPointCharacteristics.addDescriptor(&xPointDescriptor);
  
  pointsService->addCharacteristic(&yPointCharacteristics);
  yPointDescriptor.setValue("Sin(x) functions - points y"); 
  yPointCharacteristics.addDescriptor(new BLE2902()); //без него не будет работать notify
  yPointCharacteristics.addDescriptor(&yPointDescriptor);
  
  pointsService->start();

  // запускаем рассылку оповещений:
  pServer->getAdvertising()->start();
  Serial.println("Waiting a client connection to notify...");
             //  "Ждем подключения клиента, чтобы отправить уведомление..."
}

float pi = 3.141592653589793238;
const int size = 100;
float pointsX[size];
float pointsY[size];
float step = pi / 12;
float endstep = 0;

void loop() {
  if (deviceConnected) {
   for (size_t i = 0; i < size; i++)
   {
    pointsX[i] = step * i + endstep;
    pointsY[i] = sin(pointsX[i]);
    Serial.print("\nНомер индекса i: ");
    Serial.print(i+1);
    Serial.print("\tКоордината X (нужно домножить на П): ");
    Serial.print(pointsX[i]);
    Serial.print("\tКоордината Y: ");
    Serial.println(pointsY[i]);
    if (i + 1 == size) {
      endstep = endstep + step * (i + 1);
    } 
   }
    pack.SetData(pointsY);
    controller.CreatePack(pack);
    xPointCharacteristics.setValue(reinterpret_cast<uint8_t*>(&pointsX), sizeof(pointsX));
    yPointCharacteristics.setValue(reinterpret_cast<uint8_t*>(controller.GetPack()), controller.GetPacksize());
    //Проверка значений
    /*
    Serial.print("Размер указателя\n");
    Serial.print(sizeof(pointsY));
    Serial.print("\nРазмер указателя\n");
    uint8_t* temp = yPointCharacteristics.getData();
    float* arr = reinterpret_cast<float*>(temp);
    Serial.print("Размер указателя\n");
    Serial.print(sizeof(arr));
    Serial.print("\nРазмер указателя\n");
    for (size_t i = 0; i < size; i++)
    {
      Serial.print("\nЗначение i = ");
      Serial.print(i);
      Serial.print("\tЗначение переменной = ");
      Serial.print(arr[i]);
    }
    */
    xPointCharacteristics.indicate();
    yPointCharacteristics.indicate();

   
   delay(5000);
  }
  else {
    // for (size_t i = 0; i < size; i++) {
    // pointsX[i] = step * i + endstep;
    // pointsY[i] = sin(pointsX[i]);
    // } 
    // pack.SetData(pointsY);
    // controller.CreatePack(pack);
    // endstep = 0;
    Serial.println("Устройство отключено!");
    pServer->getAdvertising()->start();
    delay(2000);
  }
}
