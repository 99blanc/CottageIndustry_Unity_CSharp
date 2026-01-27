# 🍽️ Late For Dinner (레이트 포 디너)

<p align="center">
  <img src="https://capsule-render.vercel.app/render?type=waving&color=auto&height=200&section=header&text=Late%20For%20Dinner&fontSize=70" />
</p>

> **"인류가 멸망하고 수십 년... 나는 너무 늦게 소환되었다."** > 노쇠한 몬스터들의 요양원 '실버 맨션'에서 벌어지는 기묘하고 코믹한 2D 플랫포머 액션 게임.

---

## 🎮 Game Concept
본 프로젝트는 '소환의 타이밍이 어긋난 영웅'이라는 독특한 설정을 바탕으로 합니다.
* **세계관**: 마족과의 전쟁을 위해 소환되었으나, 이미 인류는 멸망하고 몬스터들만 늙어버린 시대.
* **핵심 루프**: 병원 내 NPC들과의 상호작용, 퀘스트 수행, 그리고 노쇠한 몬스터들과의 생존 전투.
* **분위기**: 음산하면서도 코믹한 분위기를 자아내는 독특한 아트워크.

---

## 🛠 High-Performance Tech Stack
최신 유니티 엔진과 고성능 라이브러리를 통해 최적화된 게임 플레이를 보장합니다.

### **Environment**
* **Engine**: Unity 6.3 LTS (6000.3.3f1)
* **IDE**: Visual Studio 2022 (17.14)
* **Version Control**: Fork 2.15

### **Modern C# Implementation**
| Library | Usage | Feature |
| :--- | :--- | :--- |
| **R3 (1.3.0)** | Reactive Framework | 캐릭터 스탯 모듈화 및 이벤트 시스템 기반 조작. |
| **UniTask (2.5.10)** | Async Logic | Zero-allocation 비동기 처리 및 시퀀스 연출. |
| **MemoryPack (1.21.4)** | Serialization | 고속 바이너리 직렬화를 통한 세이브 시스템. |
| **ZString (2.6.0)** | String Optimization | 메모리 효율 극대화 (GC 최소화). |
| **CsvHelper (33.1.0)** | Data Parsing | CSV 기반 데이터베이스 구축 및 관리. |

---

## 🕹 Key Features

### **1. 정교한 캐릭터 컨트롤**
* **조작 체계**: 방향키 이동, 스페이스바 점프(2단 점프 포함), 대쉬(Double Click).
* **상호작용**: 사다리 등반(위 방향키), 수레(Cart) 밀기, 문/포탈 이용.

### **2. 인벤토리 및 스탯 시스템**
* **Health**: 젤다 스타일 하트 시스템 (기본 3칸, 반 칸 단위 소모).
* **Inventory**: 장비/소비/기타 탭 구분 및 퀵슬롯(Z, X, C, V) 연동.
* **Currency**: 최대 9999 골드 및 아이템 가치 시스템.

### **3. 다이내믹 시나리오**
* **퀘스트**: NPC(카운터 누나 등)와의 대화 및 캔커피 사오기 등 심부름 미션 수행.
* **보스 패턴**: '늙은 고블린'의 휠체어 돌진 및 도끼 투척 패턴 회피 (30초 생존 타임아웃).

---

## 🗺️ Project Structure (Level Design)
1. **병원 입구**: 기본 조작 숙달을 위한 튜토리얼 맵.
2. **카운터/매점**: 경제 시스템 및 퀘스트 허브 구역.
3. **인간 창고**: 최초 무기 '날카로운 뼈' 획득 및 전투 시스템 활성화 지점.

---

## 👥 Contributors
* **김장욱 (nsbn)**: 기획 및 아트워크 총괄.
* **김세현 (99blanc)**: Unity/C# 메인 개발.

---

## 📜 License
This project is licensed under the **MIT License**.