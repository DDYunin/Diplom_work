#ifndef _PACKAGE_H_
#define _PACKAGE_H_


#include <string.h>
#include <stdint.h>

// Класс, который образует пакет данных
class myPackage {

private:
    
    unsigned int pack_number;
    uint16_t umv;
    float all_data[100];
public:    
    myPackage(unsigned int number_pack = 1, uint16_t value_umv = 25) 
    {
        pack_number = number_pack;
        umv = value_umv;
    }

    void SetData(float* ready_data)
    {
        for (size_t i = 0; i < 100; i++) {
            all_data[i] = ready_data[i];
            // Serial.print(all_data[i]);
            // Serial.println(ready_data[i]);
        }
        pack_number++;
        // Serial.println(pack_number);
    }

    unsigned int* GetPackNumber(){
       return &pack_number;
    }

    uint16_t* GetUMV(){
        return &umv;    
    }

    float* GetBaseData(){
        return all_data;
    }

    float* GetElemData(int i) {
        return &all_data[i];
    }


};

// Класс, который осуществляет управление пакетом данных
class PackageController {

private:
    byte pack[416];
    const int size = 416;

public:
    byte* GetPack(){
        return pack;
    }

    int GetPacksize() {
        return size;
    }

    void CreatePack(myPackage& _pack) {
        char start_mess[9] = "ECG_Data";
        char start_symbol_data[2] = "<";
        char end_symbol_data[2] = ">";
        int offset = 0;

        // Создание пакета
        for(int i = 0; i < 8; i++) {
            pack[i] = start_mess[i];
        }
        offset += 8;
        memcpy(pack + offset, _pack.GetPackNumber(), sizeof(unsigned int));
        offset += 4;
        memcpy(pack + offset, _pack.GetUMV(), sizeof(uint16_t));
        offset += 2;
        pack[offset] = start_symbol_data[0];
        offset++;
        for (int i = 0; i < 100; i++) {
            memcpy(pack + offset + i * sizeof(float), _pack.GetElemData(i), sizeof(float));
        }
        offset += 400;
        pack[offset] = end_symbol_data[0];
        Serial.println("Пакет");
        for (int i = 0; i < 416; i++) {
            Serial.print(pack[i]);
        }
        
        Serial.println("/Пакет");
        // Расшифровка пакета
        offset = 0;
        char new_start_mess[9];

        for(int i = 0; i < 8; i++) {
            new_start_mess[i] = pack[i];
        }
        offset += 8;
        Serial.println("Проверка start-mess:");
        for(int i = 0; i < 8; i++) {
            Serial.print(new_start_mess[i]);
        }
        Serial.println();
        
        Serial.println("Проверка pack-num:");
        unsigned int new_pack_num;
        memcpy(&new_pack_num, pack + offset, sizeof(unsigned int));
        Serial.println(new_pack_num);
        offset += 4;

        Serial.println("Проверка UMV:");
        uint16_t new_umv;
        memcpy(&new_umv, pack + offset, sizeof(uint16_t));
        Serial.println(new_umv);
        offset += 2;

        Serial.println("Проверка start_symbol_data:");
        char new_start_symbol_data;
        memcpy(&new_start_symbol_data, pack + offset, sizeof(char));
        Serial.println(new_start_symbol_data);
        offset += 1;

        Serial.println("Проверка float_data:");
        float new_data[100];
        for (int i = 0; i < 100; i++) {
            memcpy(&new_data[i], &pack[offset + i * sizeof(float)], sizeof(float));
            Serial.println(new_data[i]);
        }
        offset += 400;

        Serial.println("Проверка end_symbol_data:");
        char end_start_symbol_data;
        memcpy(&end_start_symbol_data, pack + offset, sizeof(char));
        Serial.println(end_start_symbol_data);

        // Serial.println("head");
        // memcpy(pack, _pack.GetStartMessage(), 8);
        // offset += 8;
        // Serial.println("num");
        // int v = _pack.GetPackNumber();
        // memcpy(&pack[offset], reinterpret_cast<uint8_t*>(&(v)), 4);
        // offset += 4;
        // uint16_t m = _pack.GetUMV();
        // memcpy(&pack[offset], reinterpret_cast<uint8_t*>(&(m)), 2);
        // Serial.println("UMV");
        // offset += 2;
        // memcpy(&pack[offset], _pack.GetStartSymbolData(), 1);
        // Serial.println("StartSymbolData");
        // offset += 1;
        // float * d = _pack.GetBaseData();
        // memcpy(&pack[offset], reinterpret_cast<uint8_t*>(&(d)), 400);
        // Serial.println("BaseData");
        // offset += 400;
        // memcpy(&pack[offset], _pack.GetEndSymbolData(), 1);
        // // перевод обратно
        // Serial.println("Проверяем данные:");
        // for(int i = 0; i < 8; i++){
        //     Serial.print((char)pack[i]);
        // }
        // Serial.println();
        // Serial.println("NumPack:");
        // int temp;
        // uint8_t* n_pack = pack + 7;
        // memcpy(&temp, reinterpret_cast<int*>(&(n_pack)), sizeof(int));
        // Serial.println(temp);
        // Serial.println("UMV:");
        // uint16_t* temp1;
        // memcpy(temp1, reinterpret_cast<uint16_t*>(pack + 12), 2);
        // for(int i = 0; i < 1; i++){
        //     Serial.println(temp[i]);
        // }
        // Serial.println("StartSymbol:");
        // for(int i = 14; i < 15; i++){
        //     Serial.print((char)pack[i]);
        // }
        // Serial.println("Data:");
        // float* temp2;
        // memcpy(temp2, reinterpret_cast<float*>(pack + 15), 400);
        // for(int i = 0; i < 100; i++){
        //     Serial.println(temp2[i]);
        // }
        // Serial.println("EndSymbol:");
        // for(int i = 415; i < 416; i++){
        //     Serial.print((char)pack[i]);
        // }




        // unsigned int current_pos = 0;
        // Serial.println("StartMessage_START_FUNCTION");
        // // memcpy(pack, _pack.GetStartMessage(), 8);
        // Serial.print("StartMessage");
        // current_pos += 8;
        // memcpy(pack + current_pos, (int*)_pack.GetPackNumber(), 4);
        // Serial.print("PackNumber");
        // current_pos += 4;
        // memcpy(pack + current_pos, (int*)_pack.GetUMV(), 2);
        // Serial.print("UMV");
        // current_pos += 2;
        // memcpy(pack + current_pos, _pack.GetStartSymbolData(), 1);
        // Serial.print("StartSymbolData");
        // current_pos += 1;
        // memcpy(pack + current_pos, _pack.GetBaseData(), 4);
        // Serial.print("BaseData");
        // current_pos += 400;
        // memcpy(pack + current_pos, _pack.GetEndSymbolData(), 1);
        // Serial.print("EndSymbolData");
        // for (size_t i = 0; i < 416; i++) {
        //     Serial.print(pack[i]);
        // }
        
    }

};
#endif