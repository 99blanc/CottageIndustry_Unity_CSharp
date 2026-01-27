# 🍽️ Late For Dinner (레이트 포 디너)

> **인류 멸망 후 소환된 주인공이 노쇠한 몬스터 요양원(실버 맨션)에서 겪는 2D 플랫포머 액션.**

---

## 🛠 Technical Stack

### **High-Performance C# Scripting**
최신 Unity 6 환경에서 가비지 컬렉션(GC) 최적화와 비동기 처리를 극대화한 스택을 사용합니다.
* **Reactive Framework**: **R3 (1.3.0)**를 이용한 반응형 스탯 및 UI 이벤트 바인딩.
* **Asynchronous Task**: **UniTask (2.5.10)** 기반의 Zero-allocation 비동기 로직 구현.
* **Fast Serialization**: **MemoryPack (1.21.4)**을 통한 초고속 세이브/로드 시스템.
* **String & Memory**: **ZString (2.6.0)**과 **ZLinq (1.5.4)**를 활용한 메모리 효율 최적화.

### **Development Environment**
* **Engine**: Unity 6.3 LTS (6000.3.3f1)
* **IDE**: Visual Studio 2022 (17.14)
* **Data Management**: CsvHelper (33.1.0)를 이용한 외부 데이터 파싱.

---

## 🎨 Art & Design Architecture

### **Visual Narrative & UI**
* **Dynamic Sprite Animation**: 투명도 조절 애니메이션을 활용한 주인공 및 합격 통지서 등장 연출.
* **Heart-Based Vitality**: 하트 반 칸 단위(0.5 Damage)로 소모되는 클래식 체력 시스템 UI.
* **Categorized Inventory**: 장비, 소비, 기타로 분류된 20칸 리드 전용/반응형 탭 UI 및 퀵슬롯 연동.

### **Level Design & Interaction**
* **Physical Objects**: RigidBody2D 기반의 밀기 가능한 수레(Cart), 사다리 등반, 가변 발판 기믹.
* **Seamless Map Transition**: 같은 카테고리 맵 간 로딩 없는 이동 및 특정 구역 포탈 상호작용.
* **Interactive Sequence**: NPC 인접 시 하얀색 테두리(Outline) 생성 및 사각 말풍선 가이드 표시.

---

## 🎮 Core Mechanics

### **Character Control**
* **Movement**: 방향키 이동 및 점프, 체공 중 아래 방향 대쉬 등 다각도 대쉬 시스템.
* **Double Jump**: 스페이스바 2연속 입력을 통한 2단 점프 조작.
* **Combat**: 최초 무기 '날카로운 뼈' 획득 후 '공격' 기능 및 무기 슬롯 활성화.

### **Enemy Patterns (Boss/Event)**
* **Aging Monsters**: 휠체어 돌진, 직선/포물선 도끼 투척 등 물리 궤적 기반의 보스 패턴.
* **Event Trigger**: 특정 지점 도달 시 플레이어 조작 불능 및 대화 시퀀스 강제 진행.

---

## 👥 Contributors
* **아트 및 기획**: 김장욱 (nsbn) - 스토리보드, UI 디자인, 시나리오 설계.
* **개발**: 김세현 (99blanc) - Unity 엔진 로직 및 C# 시스템 아키텍처 구현.

---

## 📜 License
이 프로젝트는 **MIT License**를 따릅니다.