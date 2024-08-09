class Time {
    constructor(hours = 0, minutes = 0) {
        this.Hours = hours;
        this.Minutes = minutes;
    }

    static fromString(timeString) {
        const [hours, minutes] = timeString.split(':').map(Number);
        return new Time(hours, minutes);
    }

    toString() {
        const hours = String(this.Hours).padStart(2, '0');
        const minutes = String(this.Minutes).padStart(2, '0');
        return `${hours}:${minutes}`;
    }

    difference(otherTime) {
        const thisTotalMinutes = this.Hours * 60 + this.Minutes;
        const otherTotalMinutes = otherTime.Hours * 60 + otherTime.Minutes;
        const diffMinutes = Math.abs(thisTotalMinutes - otherTotalMinutes);

        const diffHours = Math.floor(diffMinutes / 60);
        const diffRemainMinutes = diffMinutes % 60;

        return new Time(diffHours, diffRemainMinutes);
    }

    sum(otherTime) {
        const totalMinutes = (this.Hours + otherTime.Hours) * 60 + this.Minutes + otherTime.Minutes;
        const sumHours = Math.floor(totalMinutes / 60);
        const sumMinutes = totalMinutes % 60;

        return new Time(sumHours, sumMinutes);
    }

    static fromMidnight() {
        return new Time(0, 0);
    }

    static fromJsonDate(jsonDate) {
        let timestamp = parseInt(jsonDate.replace(/\/Date\((\d+)\)\//, '$1'));
        let date = new Date(timestamp);
        return new Time(date.getHours(), date.getMinutes());
    }
}