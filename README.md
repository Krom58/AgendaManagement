# ระบบจัดการวาระประชุม (Agenda Management System)

**Agenda Management System** คือเครื่องมือสำหรับ **จัดการวาระประชุม** (meeting agendas), **ลงทะเบียนผู้เข้าร่วม** (participant registration) และ **สรุปผลการลงมติ** (voting results)

## ภาพรวม (Overview)

- ลงทะเบียนผู้เข้าร่วมประชุมแบบ **เข้าร่วมด้วยตัวเอง** (In-Person) หรือ **มอบฉันทะ** (By Proxy)  
- บันทึกผลการลงมติในแต่ละวาระเป็น **เห็นด้วย** (Agree), **ไม่เห็นด้วย** (Disagree) หรือ **งดออกเสียง** (Abstain)  
- สร้างรายงานสรุป:
  - รายชื่อผู้ลงทะเบียนทั้งหมดพร้อมสถานะการเข้าร่วม  
  - ผลการลงมติของแต่ละวาระ  

ข้อมูลทั้งหมดจะถูกเก็บใน **ฐานข้อมูล** (Database) อย่างปลอดภัย

## คุณสมบัติหลัก (Key Features)

- **จัดการวาระประชุม** (Agenda Management): เพิ่ม, แก้ไข, ลบ วาระประชุมได้ง่าย  
- **ลงทะเบียนผู้เข้าร่วม** (Participant Registration): ติดตามการเข้าร่วม ทั้ง In-Person และ By Proxy  
- **จัดการการลงมติ** (Voting Management): บันทึกผลการลงมติอย่างเป็นระบบ  
- **รายงานสรุป** (Detailed Reports):
  - สรุปผู้เข้าร่วมและสถานะการเข้าร่วม (Attendance)  
  - ผลการลงมติแต่ละวาระ (Voting Outcomes)

## โครงสร้างโฟลเดอร์ (Folder Structure)

- **AgendaDetail/**: โมดูลและคอมโพเนนต์สำหรับรายละเอียดวาระและสรุปการลงมติ  
- **AgendaManagement/**: ตรรกะหลักของแอปพลิเคชันในการจัดการผู้เข้าร่วมและรายงาน  
- **database_config/**: ไฟล์ตั้งค่าการเชื่อมต่อฐานข้อมูล

## Dependencies

โปรเจกต์นี้ใช้ไลบรารีจากภายนอกดังนี้  
- **[ThaiNationalIDCard](https://github.com/dotnetthailand/ThaiNationalIDCard)**  
  ไลบรารีสำหรับอ่านและโต้ตอบกับบัตรประชาชนไทย (Thai National ID Card) ใช้ในการผสานข้อมูลประชาชนเข้ากับแอปพลิเคชัน

## ความต้องการเบื้องต้น (Prerequisites)

- **Visual Studio** (2019 หรือใหม่กว่า)  
- **.NET Framework** (ตามที่ระบุในไฟล์โซลูชัน)  
- **ตั้งค่าฐานข้อมูล** (ดูวิธีตั้งค่าในโฟลเดอร์ `database_config/`)

## การติดตั้ง (Installation)

1. Clone โปรเจกต์:
   ```bash
   git clone https://github.com/yourusername/agenda-management.git
   cd agenda-management
