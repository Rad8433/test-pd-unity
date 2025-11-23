#include <Arduino.h>
#include <MicroOscSlip.h>
#include <M5_PbHub.h>
#define CANAL_KEY_UNIT   0
#define CANAL_ANGLE_UNIT 1
M5_PbHub myPbHub;
MicroOscSlip<128> monOsc(&Serial);

void setup()
{
  Wire.begin();
  Serial.begin(115200);
  myPbHub.begin();
  myPbHub.setPixelCount(CANAL_KEY_UNIT, 0);
}

void loop()
{
  int maLectureAngle = myPbHub.analogRead(CANAL_ANGLE_UNIT);
  int rotation = maLectureAngle * 360 / 4095;
  int maLectureKey = myPbHub.digitalRead(CANAL_KEY_UNIT);
  monOsc.sendInt("/rotation", rotation);
  monOsc.sendInt("/bouton", maLectureKey);
  delay(20);
}
