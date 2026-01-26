# 🍽️ Late For Dinner (레이트 포 디너)

> **인류 멸망 수십 년 후, 뒤늦게 소환된 주인공이 노쇠한 몬스터들의 요양원에서 마주하는 기괴한 일상.**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![Unity](https://img.shields.io/badge/Unity-6000.3.3f1_LTS-black?style=for-the-badge&logo=unity)
![C#](https://img.shields.io/badge/C%23-12.0-blue?style=for-the-badge&logo=csharp)

---

## 🛠 Tech Stack (기술 스택)

본 프로젝트는 최신 유니티 환경에서 고성능 비동기 처리와 반응형 프로그래밍을 구현하기 위해 최신 라이브러리 스택을 채택하였습니다.

### **Core Engine & IDE**
* **Unity**: 6.3 LTS (6000.3.3f1)
* **IDE**: Visual Studio 2022 (17.14)
* **Version Control**: Fork 2.15

### **High-Performance Libraries (Cysharp & Open Source)**
* **Reactive Programming**: **R3 (1.3.0)** - 차세대 반응형 프레임워크로 캐릭터 스탯 및 시스템 이벤트 관리.
* **Async Logic**: **UniTask (2.5.10)** - Zero-allocation 비동기 처리를 통한 고효율 로직 구현.
* **Serialization**: **MemoryPack (1.21.4)** - 초고속 바이너리 직렬화를 통한 세이브 데이터 관리.
* **String Optimization**: **ZString (2.6.0)** - 가비지(GC) 없는 문자열 처리를 위한 Zero-allocation 라이브러리.
* **Data Handling**: **CsvHelper (33.1.0)**, **ZLinq (1.5.4)** - 효율적인 CSV 데이터 파싱 및 데이터 쿼리.

---

## 🎮 주요 게임 시스템 및 기획

### **1. 세계관 및 타이틀 의도**
* **의도**: '식사(식인)'의 전성기가 지난 후 소환된 주인공의 허망함을 'Late For Dinner'라는 제목에 담았습니다.
* **요소**: 노쇠한 몬스터를 돌보거나 처단하는 독특한 상호작용 중심의 2D 플랫폼 액션.

### **2. 게임 플레이 메커니즘**
* **캐릭터 컨트롤**: 방향키 이동, 2단 점프, 대쉬, 사다리 등 정교한 조작 체계.
* **UI/UX 시스템**: 하트 기반 체력(3칸 기본), 탭 구분형 인벤토리(20칸), 퀵슬롯(Z,X,C,V).
* **상호작용**: 밀기 가능한 수레(Cart), 사다리, 발판 등 물리 기반 오브젝트 기믹.

### **3. 시나리오 흐름**
* **튜토리얼**: 병원 입구에서 기초 조작 숙지 후 매점에서 아이템 구매 및 인벤토리 사용법 학습.
* **전투 이벤트**: 휠체어를 탄 늙은 고블린의 변칙적인 도끼 투척 패턴 회피 및 생존.

---

## 📜 License
This project is licensed under the **MIT License**.

---

## 👥 Contributors
* **Art & Design**: 김장욱 (nsbn) - [nsbn014@gmail.com]
* **Programming**: 김세현 (99blanc) - [tpgus990421@gmail.com]